using UnityEngine;

namespace Utils
{
    public class ParallaxCamera : MonoBehaviour
    {
        [SerializeField] private Camera Camera;
        [SerializeField] private float ParallaxAmount;

        private Vector3 StartPosition = Vector3.zero;
    
        private void Start()
        {
            StartPosition = transform.position;
        }

        private void FixedUpdate()
        {
            var cameraPos = Camera.transform.position;
            var distX = (cameraPos.x * ParallaxAmount);
            var distY = (cameraPos.y * ParallaxAmount);

            var objTransform = transform;
            var position = objTransform.position;
            position = new Vector3(StartPosition.x + distX, StartPosition.y + distY, position.z);
        
            objTransform.position = position;
        }
    }
}