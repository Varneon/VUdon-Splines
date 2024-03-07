using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Varneon.VUdon.Splines
{
    /// <summary>
    /// Class for creating characteristic matrices for evaluating splines.
    /// </summary>
    /// <remarks>
    /// When implementing UdonSplines manually into an UdonSharpBehaviour, use this class to construct the characteristic matrices and cache them in your class for reuse.
    /// </remarks>
    public static class SplineCharacteristicMatrixFactory
    {
        private const float
            _0F = 0f,
            _1F = 1f,
            _2F = 2f,
            _3F = 3f,
            _4F = 4f,
            _1FN = -1f,
            _2FN = -2f,
            _3FN = -3f,
            _5FN = -5f,
            _6FN = -6f;

        /// <summary>
        /// Generate cubic Bézier characteristic matrix.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating Bézier curves.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 CubicBezierMatrix() => new Matrix4x4(
            new Vector4(_1F, _0F, _0F, _0F),
            new Vector4(_3FN, _3F, _0F, _0F),
            new Vector4(_3F, _6FN, _3F, _0F),
            new Vector4(_1FN, _3F, _3FN, _1F)
        );

        /// <summary>
        /// Generate Hermite spline characteristic matrix.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating Hermite splines.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 HermiteMatrix() => new Matrix4x4(
            new Vector4(_1F, _0F, _0F, _0F),
            new Vector4(_0F, _1F, _0F, _0F),
            new Vector4(_3FN, _2FN, _3F, _1FN),
            new Vector4(_2F, _1F, _2FN, _1F)
        );

        /// <summary>
        /// Generate Hermite spline characteristic matrix.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating Hermite splines.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 CatmullRomMatrix() => new Matrix4x4(
            new Vector4(_0F, _2F, _0F, _0F),
            new Vector4(_1FN, _0F, _1F, _0F),
            new Vector4(_2F, _5FN, _4F, _1FN),
            new Vector4(_1FN, _3F, _3FN, _1F)
        );

        /// <summary>
        /// Generate B-Spline characteristic matrix.
        /// </summary>
        /// <returns>Matrix to be cached and used for evaluating B-Splines.</returns>
        [PublicAPI]
        [SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public static Matrix4x4 BSplineMatrix() => new Matrix4x4(
            new Vector4(_1F, _4F, _1F, _0F),
            new Vector4(_3FN, _0F, _3F, _0F),
            new Vector4(_3F, _6FN, _3F, _0F),
            new Vector4(_1FN, _3F, _3FN, _1F)
        );
    }
}
