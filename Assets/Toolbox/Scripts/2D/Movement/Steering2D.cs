using UnityEngine;

namespace Toolbox
{
    public struct Steering2D
    {
        public Vector2 force;
        public bool isMoving;

        /// <summary>
        /// A steering with no force that is not considered moving so the rigidbody receives stopping drag.
        /// </summary>
        public static readonly Steering2D Stop = new Steering2D
        {
            force = Vector2.zero,
            isMoving = false
        };

        /// <summary>
        /// A steering with no force, but is still considered moving so the rigidbody receives moving drag.
        /// </summary>
        public static readonly Steering2D None = new Steering2D
        {
            force = Vector2.zero,
            isMoving = true
        };

        public static Steering2D operator +(Steering2D left, Steering2D right)
        {
            return new Steering2D
            {
                force = left.force + right.force,
                isMoving = left.isMoving || right.isMoving
            };
        }
    }
}