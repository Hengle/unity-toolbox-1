﻿using UnityEngine;

namespace Toolbox
{
    public static class Utils
    {
        public const float EPSILON = 0.00001f;

        public static T GetComponentAtMouse3D<T>()
        {
            T target = default(T);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                target = hit.transform.GetComponent<T>();
            }

            return target;
        }

        public static T GetComponentAtMouse2D<T>()
        {
            T target = default(T);

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(worldPoint);

            if (col != null)
            {
                target = col.GetComponent<T>();
            }

            return target;
        }

        public static bool IsZero(float f)
        {
            return System.Math.Abs(f) < EPSILON;
        }

        public static float Sign(float f)
        {
            return IsZero(f) ? 0 : Mathf.Sign(f);
        }

        /// <summary>
        /// Converts the vector to an angle in radians.
        /// </summary>
        public static float Vector2ToAngle(Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x);
        }

        /// <summary>
        /// Converts the angle in radians to a vector.
        /// </summary>
        public static Vector2 AngleToVector2(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public static float RandomBinomial()
        {
            return Random.value - Random.value;
        }

        /// <summary>
        /// Casts a 2D ray with queriesStartInColliders set to false for just
        /// the raycast without affecting the project setting.
        /// </summary>
        public static RaycastHit2D Raycast2D(Vector2 origin, Vector2 direction, float distance, int layerMask)
        {
            bool origQueriesStartInColliders = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = false;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

            Physics2D.queriesStartInColliders = origQueriesStartInColliders;

            return hit;
        }

        /// <summary>
        /// Casts a circle with queriesStartInColliders set to false for just
        /// the circle cast without affecting the project setting.
        /// </summary>
        public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
        {
            bool origQueriesStartInColliders = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = false;

            RaycastHit2D hit = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);

            Physics2D.queriesStartInColliders = origQueriesStartInColliders;

            return hit;
        }

        public static void DebugDrawCross(Vector3 position, float size, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawRay(position - Vector3.right * size / 2, Vector3.right * size, color, duration, depthTest);
            Debug.DrawRay(position - Vector3.down * size / 2, Vector3.down * size, color, duration, depthTest);
            Debug.DrawRay(position - Vector3.forward * size / 2, Vector3.forward * size, color, duration, depthTest);
        }
    }
}