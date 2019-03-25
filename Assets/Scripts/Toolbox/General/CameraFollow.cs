using UnityEngine;

namespace Toolbox
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float followPercent = 0.1f;
        public bool autoOffset = true;
        public Vector3 offset;

        const float expectedFPS = 60f;

        public virtual void Start()
        {
            if (target == null)
            {
                Debug.Log("CameraFollow has no target.");
            }
            else if (autoOffset)
            {
                offset = transform.position - target.position;
            }
        }

        public virtual void LateUpdate()
        {
            transform.position = GetSmoothedPosition();
        }

        public Vector3 GetSmoothedPosition()
        {
            Vector3 pos = transform.position;

            if (target != null)
            {
                Vector3 desiredPos = target.position + offset;
                pos += (desiredPos - pos) * followPercent * (Time.deltaTime * expectedFPS);
            }

            return pos;
        }
    }
}