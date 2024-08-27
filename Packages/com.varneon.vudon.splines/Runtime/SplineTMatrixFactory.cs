using UnityEngine;

namespace Varneon.VUdon.Splines
{
    /// <summary>
    /// Class for constructing T matrices for evaluating splines.
    /// </summary>
    public static class SplineTMatrixFactory
    {
        private const float
            _0F = 0f,
            _1F = 1f,
            _2F = 2f,
            _3F = 3f,
            _6F = 6f;

        /// <summary>
        /// Generates a T matrix for evaluating position.
        /// </summary>
        /// <remarks>
        /// [1 t t² t³]
        /// </remarks>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>T Matrix for evaluating position.</returns>
        public static Vector4 GeneratePositionMatrix(float t)
        {
            float tt = t * t;

            return new Vector4(_1F, t, tt, tt * t);
        }

        /// <summary>
        /// Generates a T matrix for evaluating velocity.
        /// </summary>
        /// <remarks>
        /// [0 1 2t 3t²]
        /// </remarks>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>T Matrix for evaluating velocity.</returns>
        public static Vector4 GenerateVelocityMatrix(float t)
        {
            return new Vector4(_0F, _1F, _2F * t, _3F * t * t);
        }

        /// <summary>
        /// Generates a T matrix for evaluating acceleration.
        /// </summary>
        /// <remarks>
        /// [0 0 2 6t]
        /// </remarks>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>T Matrix for evaluating acceleration.</returns>
        public static Vector4 GenerateAccelerationMatrix(float t)
        {
            return new Vector4(_0F, _0F, _2F, _6F * t);
        }

        /// <summary>
        /// Generates a T matrix for evaluating jolt.
        /// </summary>
        /// <remarks>
        /// [0 0 0 6]
        /// </remarks>
        /// <returns>T Matrix for evaluating jolt.</returns>
        public static Vector4 GenerateJoltMatrix()
        {
            return new Vector4(_0F, _0F, _0F, _6F);
        }

        /// <summary>
        /// Updates a T matrix for evaluating position.
        /// </summary>
        /// <remarks>
        /// [1 t t² t³]
        /// </remarks>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>T Matrix for evaluating position.</returns>
        public static void SetPositionMatrix(ref Vector4 tMatrix, float t)
        {
            float tt = t * t;

            tMatrix.Set(_1F, t, tt, tt * t);
        }

        /// <summary>
        /// Updates a T matrix for evaluating velocity.
        /// </summary>
        /// <remarks>
        /// [0 1 2t 3t²]
        /// </remarks>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>T Matrix for evaluating velocity.</returns>
        public static void SetVelocityMatrix(ref Vector4 tMatrix, float t)
        {
            tMatrix.Set(_0F, _1F, _2F * t, _3F * t * t);
        }

        /// <summary>
        /// Updates a T matrix for evaluating acceleration.
        /// </summary>
        /// <remarks>
        /// [0 0 2 6t]
        /// </remarks>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>T Matrix for evaluating acceleration.</returns>
        public static void SetAccelerationMatrix(ref Vector4 tMatrix, float t)
        {
            tMatrix.w = _6F * t;
        }
    }
}
