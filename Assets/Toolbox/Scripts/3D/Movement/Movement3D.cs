using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement3D : MonoBehaviour
    {
        public float accel = 35f;
        public float stoppingDrag = 50f;
        public float movingDrag;
        public float maxSpeed = 4f;
        public bool movesOnGround;

        /// <summary>
        /// The steering force that the game object should move each FixedUpdate.
        /// </summary>
        /// <remarks>
        /// The steering force will be applied every FixedUpdate so make sure you
        /// keep it correct by setting it every FixedUpdate or be sure you want
        /// it applied for all future FixedUpdates if you set it once.
        /// </remarks>
        [System.NonSerialized]
        public Steering3D steering = Steering3D.Stop;

        Rigidbody rb;
        List<ForceTime3D> knockbacks = new List<ForceTime3D>();

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            knockbacks.RemoveAll(ft => ft.endTime <= Time.fixedTime);

            if (!steering.isMoving && knockbacks.Count == 0)
            {
                rb.drag = stoppingDrag;
            }
            else
            {
                rb.drag = movingDrag;

                /* If there are knockbacks on the character then only apply
                 * those forces and not the character movement force. */
                if (knockbacks.Count > 0)
                {
                    foreach (ForceTime3D ft in knockbacks)
                    {
                        rb.AddForce(ft.force);
                    }
                }
                else
                {
                    rb.AddForce(steering.force);
                }
            }

            if (knockbacks.Count == 0)
            {
                LimitSpeed();
            }
        }

        void LimitSpeed()
        {
            Vector3 velocity = rb.velocity;

            if (movesOnGround)
            {
                velocity.y = 0;
            }

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;

                if (movesOnGround)
                {
                    velocity.y = rb.velocity.y;
                }

                rb.velocity = velocity;
            }
        }

        public void AddKnockback(Vector3 force, float duration)
        {
            ForceTime3D ft = new ForceTime3D(force, Time.fixedTime + duration);
            knockbacks.Add(ft);
        }
    }

    struct ForceTime3D
    {
        public Vector3 force;
        public float endTime;

        public ForceTime3D(Vector3 force, float endTime)
        {
            this.force = force;
            this.endTime = endTime;
        }
    }
}