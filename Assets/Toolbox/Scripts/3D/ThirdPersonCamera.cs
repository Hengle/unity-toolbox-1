using UnityEngine;

namespace Toolbox
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public float distance = 5f;
        public float sensitivityX = 4f;
        public float sensitivityY = 1f;
        public float minY = 5f;
        public float maxY = 50f;

        public Transform target;
        public Vector3 targetOffset = new Vector3(0f, 1f, 0f);

        float currentX;
        float currentY;

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                currentX += Input.GetAxis("Mouse X");
                currentY += -1 * Input.GetAxis("Mouse Y");

                currentY = Mathf.Clamp(currentY, minY, maxY);
            }
        }

        void LateUpdate()
        {
            if (target != null)
            {
                Vector3 dir = new Vector3(0, 0, -distance);
                Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                transform.position = target.position + targetOffset + (rotation * dir);
                transform.LookAt(target.position + targetOffset);
            }
        }
    }
}