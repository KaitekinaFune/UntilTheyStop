using UnityEngine;

namespace Utils
{
    public class ParallaxMouse : MonoBehaviour
    {
        [SerializeField] private float ParallaxAmount;
        [SerializeField] private Camera Camera;

        private Vector3 StartPosition = Vector3.zero;

        private void Start()
        {
            StartPosition = transform.position;
        }

        private void FixedUpdate()
        {
            var mousePos = Input.mousePosition;
            
            if (mousePos.x < 0 || mousePos.x >= Screen.width
            || mousePos.y < 0 || mousePos.y >= Screen.height)
                return;
            
            var mousePosWorld = Camera.ScreenToWorldPoint(mousePos);
            var distX = (-mousePosWorld.x * ParallaxAmount);
            var distY = (-mousePosWorld.y * ParallaxAmount);

            var objTransform = transform;
            var position = objTransform.position;
            position = new Vector3(StartPosition.x + distX, StartPosition.y + distY, position.z);

            objTransform.position = position;
        }
    }
}