using System;
using LivingEntities.Player;
using Pools;
using UnityEngine;

namespace Effects
{
    public class Arrow : MonoBehaviour, IResettable
    {
        [SerializeField] private float Speed;
        [SerializeField] private float LifeTime;
        [SerializeField] private LayerMask LayerMask;

        private float Damage;
        private float TimeAlive;
        public EventHandler ProjectileDestroyed;
    
        public void Launch(Vector3 start, Vector3 towards, float damage)
        {
            gameObject.SetActive(true);

            Damage = damage;
            TimeAlive = 0f;
        
            var t = transform;
            t.position = start;
        
            var vectorToTarget = t.position - towards;
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90f;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            t.rotation = q;
        }
        
        private void Update()
        {
            if (!gameObject.activeSelf)
                return;
        
            if (TimeAlive >= LifeTime)
                ProjectileDestroyed?.Invoke(this, null);

            TimeAlive += Time.deltaTime;
            var moveDistance = Speed * Time.deltaTime;
            CheckCollisions(moveDistance);
            gameObject.transform.Translate(Vector3.up * moveDistance);
        }

        private void CheckCollisions(float moveDistance)
        {
            var t = gameObject.transform;
            var position = t.position;
            var hit = Physics2D.Raycast(position, t.up, moveDistance, LayerMask);
        
            if (hit.collider != null)
                OnHitObject(hit.collider.gameObject);
        }

        private void OnHitObject(GameObject hit)
        {
            var player = hit.GetComponent<Player>();

            if (player != null)
                player.TakeDamage(Damage);

            ProjectileDestroyed?.Invoke(this, null);
        }

        public void Reset()
        {
            gameObject.SetActive(false);
        }
    }
}
