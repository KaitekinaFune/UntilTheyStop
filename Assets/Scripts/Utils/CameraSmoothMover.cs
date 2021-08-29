using LivingEntities.Player;
using UnityEngine;

namespace Utils
{
    public class CameraSmoothMover : MonoBehaviour
    {
        [SerializeField] private float SmoothAmount;
        [SerializeField] private float ZPos;
    
        private PlayerManager PlayerManager;
        private Vector2 Velocity;
    
        private void Start()
        {
            PlayerManager = PlayerManager.Instance;
        }

        private void FixedUpdate()
        {
            var desiredPosition = (Vector2)PlayerManager.GetPlayerPosition() + PlayerManager.GetPlayerLookDirection();
            var direction =
                Vector2.SmoothDamp(transform.position, desiredPosition, ref Velocity, SmoothAmount);
            transform.position = new Vector3(direction.x, direction.y, ZPos);
        }
    }
}
