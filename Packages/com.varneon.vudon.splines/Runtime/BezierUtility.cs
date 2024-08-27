using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Varneon.VUdon.Splines
{
    /// <summary>
    /// A collection of methods for extracting information about Bezier curves.
    /// </summary>
    public static class BezierUtility
    {
        private const float
            _0F = 0f,
            _1F = 1f,
            _3F = 3f,
            _6F = 6f,
            _9F = 9f,
            _12F = 12f,
            _18F = 18f,
            _1FN = -1f,
            _3FN = -3f,
            _6FN = -6f,
            _9FN = -9f,
            _12FN = -12f,
            _18FN = -18f;

        /// <summary>
        /// Generate cubic Bezier characteristic matrix for evaluating position on a curve.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating position.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 CubicBezierPositionMatrix() => new Matrix4x4(
            new Vector4(_1F, _0F, _0F, _0F),
            new Vector4(_3FN, _3F, _0F, _0F),
            new Vector4(_3F, _6FN, _3F, _0F),
            new Vector4(_1FN, _3F, _3FN, _1F)
        );

        /// <summary>
        /// Generate cubic Bezier characteristic matrix for evaluating tangent on a curve.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating tangent.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 CubicBezierTangentMatrix() => new Matrix4x4(
            new Vector4(_3FN, _3F, _0F, _0F),
            new Vector4(_6F, _12FN, _6F, _0F),
            new Vector4(_3FN, _9F, _9FN, _3F),
            Vector4.zero
        );

        /// <summary>
        /// Generate cubic Bezier characteristic matrix for evaluating acceleration on a curve.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating acceleration.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 CubicBezierAccelerationMatrix() => new Matrix4x4(
            new Vector4(_6F, _12FN, _6F, _0F),
            new Vector4(_6FN, _18F, _18FN, _6F),
            Vector4.zero,
            Vector4.zero
        );

        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 GenerateCachedPointMatrix(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) => new Matrix4x4(p0, p1, p2, p3);

        /// <summary>
        /// Given a Bezier curve, return an interpolated position at ratio t.
        /// </summary>
        /// <remarks>
        /// Evaluates position using Vector3.Lerp which minimizes externs, resulting in more performant calculation in Udon.
        /// </remarks>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePosition(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 q1 = Vector3.Lerp(p1, p2, t);

            return Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(p0, p1, t), q1, t), Vector3.Lerp(q1, Vector3.Lerp(p2, p3, t), t), t);
        }

        /// <summary>
        /// Given a Bezier curve, return an interpolated position at ratio t.
        /// </summary>
        /// <remarks>
        /// Evaluation using cached matrices in some cases can be up to 25% faster in UdonVM than any other method I know, but the matrices of knots that have moved must be regenerated manually before evaluation.
        /// </remarks>
        /// <param name="bezierPositionMatrix">Cached Bezier curve characteristic matrix.</param>
        /// <param name="pointMatrix">Matrix constructed of all points on the Bezier curve.</param>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionCached(Matrix4x4 bezierPositionMatrix, Matrix4x4 pointMatrix, float t)
        {
            float tt = t * t;

            return pointMatrix * bezierPositionMatrix * new Vector4(_1F, t, tt, tt * t);
        }

        [PublicAPI]
        public static Vector3 EvaluateVelocityTraditional(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float tt = t * t;

            return
                p0 * (_3FN * tt + _6F * t - _3F) +
                p1 * (_9F * tt - _12F * t + _3F) +
                p2 * (_9FN * tt + _6F * t) +
                p3 * (_3F * tt);
        }

        [PublicAPI]
        public static Vector3 EvaluateVelocity(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 v1 = p2 - p1;

            return Vector3.Lerp(Vector3.Lerp(p1 - p0, v1, t), Vector3.Lerp(v1, p3 - p2, t), t);
        }

        [PublicAPI]
        public static Vector3 EvaluateAcceleration(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return
                p0 * (_6FN * t + _6F) +
                p1 * (_18F * t - _12F) +
                p2 * (_18FN * t + _6F) +
                p3 * (_6F * t);
        }

        /// <summary>
        /// Given a Bézier curve, evaluate position, velocity, acceleration or jolt.
        /// </summary>
        /// <param name="bezierPositionMatrix">Cached Bézier curve characteristic matrix.</param>
        /// <param name="pointMatrix">Matrix constructed of all points on the Bézier curve.</param>
        /// <param name="tMatrix">T matrix.</param>
        /// <returns>Evaluated position, velocity, acceleration or jolt on the curve.</returns>
        [PublicAPI]
        public static Vector3 Evaluate(Matrix4x4 bezierPositionMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * bezierPositionMatrix * tMatrix;
        }

        /// <summary>
        /// Given a Bezier curve, return an interpolated position at ratio t using Bernstein polynomials.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionBernstein(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float tt = t * t;
            float ttt = tt * t;

            return
                p0 * (-ttt + 3f * tt - 3f * t + 1f) +
                p1 * (3f * ttt - 6f * tt + 3f * t) +
                p2 * (-3f * ttt + 3f * tt) +
                p3 * ttt;
        }
    }
}
