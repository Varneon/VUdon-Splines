using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Varneon.VUdon.Splines
{
    public static class BSplineUtility
    {
        private const float
            _0F = 0f,
            _1F = 1f,
            _2F = 2f,
            _3F = 3f,
            _4F = 4f,
            _1FN = -1f,
            _3FN = -3f,
            _6FN = -6f;

        private const float SIXTH = 1f / 6f;

        /// <summary>
        /// Generate B-Spline characteristic matrix for evaluating position on a curve.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating position.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 BSplinePositionMatrix() => new Matrix4x4(
            new Vector4(_1F, _4F, _1F, _0F),
            new Vector4(_3FN, _0F, _3F, _0F),
            new Vector4(_3F, _6FN, _3F, _0F),
            new Vector4(_1FN, _3F, _3FN, _1F)
        );

        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 GenerateCachedPointMatrix(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) => new Matrix4x4(p0, p1, p2, p3);

        /// <summary>
        /// Given a B-Spline curve, return an interpolated position at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionCached(Matrix4x4 bSplinePositionMatrix, Matrix4x4 pointMatrix, float t)
        {
            float tt = t * t;

            return pointMatrix * bSplinePositionMatrix * new Vector4(_1F, t, tt, tt * t) * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline curve, return an interpolated position at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePosition(Matrix4x4 bSplinePositionMatrix, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Matrix4x4 pointMatrix = GenerateCachedPointMatrix(p0, p1, p2, p3);

            float tt = t * t;

            return (pointMatrix * bSplinePositionMatrix * new Vector4(_1F, t, tt, tt * t)) * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline curve, return an interpolated tangent at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A tangent on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluateVelocityCached(Matrix4x4 bSplinePositionMatrix, Matrix4x4 pointMatrix, float t)
        {
            return pointMatrix * bSplinePositionMatrix * new Vector4(_0F, _1F, _2F * t, _3F * t * t) * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline curve, return an interpolated position at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionCached(Matrix4x4 bSplinePositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * bSplinePositionMatrix * tMatrix * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline curve, return an interpolated velocity at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A velocity on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluateVelocityCached(Matrix4x4 bSplinePositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * bSplinePositionMatrix * tMatrix * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline curve, return an interpolated acceleration at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>An acceleration on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluateAccelerationCached(Matrix4x4 bSplinePositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * bSplinePositionMatrix * tMatrix * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline curve, return an interpolated jolt at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A jolt on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluateJoltCached(Matrix4x4 bSplinePositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * bSplinePositionMatrix * tMatrix * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline, evaluate position, velocity, acceleration or jolt.
        /// </summary>
        /// <param name="bezierPositionMatrix">Cached B-Spline characteristic matrix.</param>
        /// <param name="pointMatrix">Matrix constructed of all points on the B-Spline curve.</param>
        /// <param name="tMatrix">T matrix.</param>
        /// <returns>Evaluated position, velocity, acceleration or jolt on the curve.</returns>
        [PublicAPI]
        public static Vector3 Evaluate(Matrix4x4 bSplinePositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * bSplinePositionMatrix * tMatrix * SIXTH;
        }

        /// <summary>
        /// Given a B-Spline curve, return an interpolated position at ratio t using Bernstein polynomials.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionBernstein(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float tt = t * t;
            float ttt = tt * t;

            return
                (p0 * (-ttt + 3f * tt - 3f * t + 1f) +
                p1 * (3f * ttt - 6f * tt + 3f * t) +
                p2 * (-3f * ttt + 3f * t) +
                p3 * (ttt + 4f * tt + t)) * SIXTH;
        }
    }
}
