using UnityEngine;

namespace Toolbox
{
    //TODO Start raycasting from the target + offset to the desired camera position and move the camera closer if it hits anything
    //TODO add zoom in / out for camera
    public class ThirdPersonCamera : MonoBehaviour
    {
        public float distance = 5f;

        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = false;

        public float minY = 0f;
        public float maxY = 89.9f;

        //TODO autoset? maybe just log error message?
        public Transform target;
        //TODO better name?
        public Vector3 targetOffset = new Vector3(0f, 1f, 0f);

        float currentX;
        float currentY;
        float targetX;
        // float targetY;

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
                Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

                float mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                currentX += mouseMovement.x * mouseSensitivityFactor;
                currentY += mouseMovement.y * mouseSensitivityFactor;

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