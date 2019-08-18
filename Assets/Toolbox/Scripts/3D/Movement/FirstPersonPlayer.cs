using Toolbox;
using UnityEngine;

namespace Toolbox
{
    /* Be sure to set gravity to be faster than real gravity (something like -30f)
     * in the Physics settings. */
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Movement3D))]
    public class FirstPersonPlayer : MonoBehaviour
    {
        public string horAxisName = "Horizontal";
        public string vertAxisName = "Vertical";
        public string jumpButtonName = "Jump";
        public Transform playerCamera;
        public float jumpAccel = 500f;

        [Header("Ground Check")]
        public LayerMask whatIsGround = Physics.DefaultRaycastLayers;
        public float groundCheckDist = 0.2f;

        CapsuleCollider capsule;
        float capsuleRadius;
        float touchingGroundDist;
        Rigidbody rb;
        Movement3D movement;
        float horAxis;
        float vertAxis;
        bool shouldJump;

        void Start()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>().transform;
            }

            capsule = GetComponent<CapsuleCollider>();
            capsuleRadius = Mathf.Max(transform.localScale.x, transform.localScale.z) * capsule.radius;
            touchingGroundDist = (transform.localScale.y * capsule.height / 2f) - capsuleRadius + Physics.defaultContactOffset;

            rb = GetComponent<Rigidbody>();
            movement = GetComponent<Movement3D>();
        }

        void Update()
        {
            horAxis = Input.GetAxisRaw(horAxisName);
            vertAxis = Input.GetAxisRaw(vertAxisName);

            if (Input.GetButtonDown(jumpButtonName))
            {
                shouldJump = true;
            }
        }

        void FixedUpdate()
        {
            float distToGround = GetDistToGround();

            Vector3 right = Vector3.ProjectOnPlane(playerCamera.right, Vector3.up).normalized;
            Vector3 forward = Vector3.ProjectOnPlane(playerCamera.forward, Vector3.up).normalized;
            Vector3 dir = (right * horAxis + forward * vertAxis).normalized;

            Vector3 accel = movement.accel * dir;

            if (distToGround <= (touchingGroundDist + groundCheckDist) && shouldJump)
            {
                accel.y = jumpAccel;
            }

            movement.steering.force = rb.mass * accel;

            movement.steering.isMoving = dir != Vector3.zero || distToGround > touchingGroundDist || accel.y > 0f;

            shouldJump = false;
        }

        float GetDistToGround()
        {
            float distToGround = Mathf.Infinity;

            Vector3 origin = transform.TransformPoint(capsule.center);
            float maxDist = touchingGroundDist + groundCheckDist;

            RaycastHit[] hits = Utils.SphereCastAll(origin, capsuleRadius, Vector3.down, maxDist, whatIsGround);

            foreach (RaycastHit hit in hits)
            {
                /* For colliders that overlap the sphere at the start of the sweep the hit
                 * distance is 0, but the ground will never overlap the sphere at the center
                 * of the capsule. */
                if (hit.distance > 0 && hit.distance < distToGround)
                {
                    distToGround = hit.distance;
                }
            }

            return distToGround;
        }
    }
}