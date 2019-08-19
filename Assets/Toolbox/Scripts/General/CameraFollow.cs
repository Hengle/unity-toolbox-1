using UnityEngine;

namespace Toolbox
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        [Tooltip("Percent of the way to the target it should follow."), Range(0.001f, 1f)]
        public float followPercent = 0.1f;
        [Tooltip("Time it takes to follow the target the follow percent."), Range(0.001f, 1f)]
        public float followLerpTime = 1f / 60f;
        public bool autoOffset = true;
        public Vector3 offset;

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
                float t = 1f - Mathf.Exp((Mathf.Log(1f - followPercent) / followLerpTime) * Time.deltaTime);
                pos += (desiredPos - pos) * t;
            }

            return pos;
        }
    }
}