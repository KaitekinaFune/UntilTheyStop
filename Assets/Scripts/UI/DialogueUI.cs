using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using AudioType = Managers.AudioType;
using Random = UnityEngine.Random;

namespace UI
{
    public class DialogueUI: MonoBehaviour
    {
        [TextArea(5, 20)] [SerializeField] private string DialogueTextString;
        [SerializeField] private string SkipTextString;
        [SerializeField] private TextMeshProUGUI DialogueText;
        [SerializeField] private TextMeshProUGUI SkipText;
        
        [SerializeField] private float TypeDelay;
        [SerializeField] private float PunctuationTypeDelay;
        [SerializeField] private float SkipTextShowAnimationLength;

        [SerializeField] private float AudioCooldown;
        [SerializeField] private float AudioPitchMin;
        [SerializeField] private float AudioPitchMax;

        [SerializeField] private UnityEvent OnDialogueClosed;

        private Color StartingSkipTextColor;
        private bool SkipTextShowed;

        private void Start()
        {
            StartingSkipTextColor = SkipText.color;
            SkipText.SetText(SkipTextString);
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(StartType));
        }

        private IEnumerator StartType()
        {
            DialogueText.SetText("");
            var lastAudioPlayTime = Time.time;
            foreach (var c in DialogueTextString)
            {
                var delay = TypeDelay;
                DialogueText.text += c;
                if (c == ',' || c == '.')
                {
                    delay += PunctuationTypeDelay;
                }
                else if (Time.time >= lastAudioPlayTime + AudioCooldown)
                {
                    lastAudioPlayTime = Time.time;
                    var pitch = Random.Range(AudioPitchMin, AudioPitchMax);
                    AudioManager.Instance.Play(AudioType.Dialogue, pitch);
                }

                yield return new WaitForSeconds(delay);
            }

            StartCoroutine(ShowSkipText());
        }

        private IEnumerator ShowSkipText()
        {
            SkipTextShowed = true;
            var t = 0f;
            var colorFrom = StartingSkipTextColor;
            colorFrom.a = 0f;
            var colorTo = StartingSkipTextColor;
            colorTo.a = 1f;
            
            for (; t <= SkipTextShowAnimationLength; t += Time.deltaTime)
            {
                var percent = Mathf.Lerp(0f, SkipTextShowAnimationLength, t);
                SkipText.color = Color.Lerp(colorFrom, colorTo, percent);
                yield return null;
            }
        }

        public void OnDialogueSkipped()
        {
            if (!SkipTextShowed)
                SkipDialogue();
            else
                CloseDialogue();
            
            OnDialogueClosed?.Invoke();
        }

        private void SkipDialogue()
        {
            StopCoroutine(nameof(StartType));
            DialogueText.text = DialogueTextString;
            StartCoroutine(ShowSkipText());
        }

        private void CloseDialogue()
        {
            gameObject.SetActive(false);
            OnDialogueClosed?.Invoke();
        }
    }
}