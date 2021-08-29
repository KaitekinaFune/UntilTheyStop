using System.Collections.Generic;
using System.Linq;
using Audio;
using LivingEntities.Player;
using UnityEngine;
using Utils;
using AudioType = Audio.AudioType;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private List<AudioData> AudioDatas;
        
        private readonly List<AudioHandler> AudioHandlers = new List<AudioHandler>();
        
        private void Start()
        {
            foreach (var audioHandler in AudioDatas.Select(audioData => 
                new AudioHandler(audioData, gameObject.AddComponent<AudioSource>())))
            {
                AudioHandlers.Add(audioHandler);
            }
        }

        private void Play(AudioType type)
        {
            foreach (var audioHandler in AudioHandlers.Where(audioHandler => audioHandler.Type == type))
            {
                audioHandler.Play();
                break;
            }
        }

        public void OnPlayerAttack(PlayerAttackType attackType)
        {
            switch (attackType)
            {
                case PlayerAttackType.Sword:
                    Play(AudioType.PlayerSwordAttack);
                    break;
                case PlayerAttackType.Dash:
                    Play(AudioType.PlayerDashAttack);
                    break;
                case PlayerAttackType.Ranged:
                    Play(AudioType.PlayerRangedAttack);
                    break;
            }
        }

        public void OnPlayerDeath()
        {
            Play(AudioType.PlayerDeath);
        }

        public void OnPlayerTakeHit()
        {
            Play(AudioType.PlayerHit);
        }

        public void OnDialogueCharacter()
        {
            Play(AudioType.Dialogue);
        }

        public void OnEnemySpawn()
        {
            Play(AudioType.EnemySpawn);
        }

        public void OnEnemyTakeHit()
        {
            Play(AudioType.EnemyHit);
        }

        public void OnEnemyDeath()
        {
            Play(AudioType.EnemyDeath);
        }
    }
}