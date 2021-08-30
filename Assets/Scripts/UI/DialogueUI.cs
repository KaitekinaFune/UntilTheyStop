using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField] private UnityEvent OnDialogueClosed;
        [SerializeField] private UnityEvent OnCharacterAppeared;

        private Color StartingSkipTextColor;
        private bool SkipTextShowed;

        private void Start()
        {
            StartingSkipTextColor = SkipText.color;
            SkipText.SetText(SkipTextString);
        }

        private void OnEnable()
        {
            SkipText.color = new Color(SkipText.color.r, SkipText.color.g, SkipText.color.b, 0f);
            SkipTextShowed = false;
            StartCoroutine(nameof(StartType));

            InputManager.Instance.AnyKeyPressed += TrySkipDialogue;
        }

        private void OnDisable()
        {
            InputManager.Instance.AnyKeyPressed -= TrySkipDialogue;
        }

        private IEnumerator StartType()
        {
            DialogueText.SetText("");
            
            foreach (var c in DialogueTextString)
            {
                var delay = TypeDelay;
                DialogueText.text += c;
                if (c == ',' || c == '.')
                    delay += PunctuationTypeDelay;

                OnCharacterAppeared?.Invoke();
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

        private void TrySkipDialogue()
        {
            if (!SkipTextShowed)
                SkipDialogue();
            else
                CloseDialogue();
        }

        private void SkipDialogue()
        {
            StopCoroutine(nameof(StartType));
            DialogueText.text = DialogueTextString;
            StartCoroutine(ShowSkipText());
        }

        private void CloseDialogue()
        {
            StopCoroutine(nameof(StartType));
            StopCoroutine(nameof(ShowSkipText));
            OnDialogueClosed?.Invoke();
        }
    }
}