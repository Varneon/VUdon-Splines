using JetBrains.Annotations;
using UnityEngine;

namespace Varneon.VUdon.Splines
{
    public static class HermiteSplineUtility
    {
        /// <summary>
        /// Given a hermite spline curve, return an interpolated position at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionCached(Matrix4x4 hermiteSplinePositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * hermiteSplinePositionMatrix * tMatrix;
        }

        /// <summary>
        /// Given a Hermite curve, evaluate position, velocity, acceleration or jolt.
        /// </summary>
        /// <param name="hermiteSplinePositionMatrix">Characteristic matrix of Hermite spline.</param>
        /// <param name="pointMatrix">Matrix constructed of all points on the Hermite curve.</param>
        /// <param name="tMatrix">T matrix.</param>
        /// <returns>Evaluated position, velocity, acceleration or jolt on the curve.</returns>
        [PublicAPI]
        public static Vector3 Evaluate(Matrix4x4 hermiteSplinePositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * hermiteSplinePositionMatrix * tMatrix;
        }

        /// <summary>
        /// Given a Hermite curve, return an interpolated position at ratio t using Bernstein polynomials.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionBernstein(Vector3 p0, Vector3 v0, Vector3 p1, Vector3 v1, float t)
        {
            float tt = t * t;
            float ttt = tt * t;

            return
                p0 * (2f * ttt + tt - 2f * t + 1f) +
                v0 * (-3f * ttt - 2f * tt + 3f * t - 1f) +
                p1 * tt +
                v1 * ttt;
        }
    }
}
