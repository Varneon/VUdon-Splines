using JetBrains.Annotations;
using UnityEngine;

namespace Varneon.VUdon.Splines
{
    public class CatmullRomUtility : MonoBehaviour
    {
        private const float HALF = 1f / 2f;

        /// <summary>
        /// Given a Catmull-Rom curve, return an interpolated position at ratio t.
        /// </summary>
        /// <param name="t">A value between 0 and 1 representing the ratio along the curve.</param>
        /// <returns>A position on the curve.</returns>
        [PublicAPI]
        public static Vector3 EvaluatePositionCached(Matrix4x4 catmullRomCharacteristicMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * catmullRomCharacteristicMatrix * tMatrix * HALF;
        }

        /// <summary>
        /// Given a Catmull-Rom curve, evaluate position, velocity, acceleration or jolt.
        /// </summary>
        /// <param name="catmullRomCharacteristicMatrix">Characteristic matrix of Catmull-Rom.</param>
        /// <param name="pointMatrix">Matrix constructed of all points on the Catmull-Rom curve.</param>
        /// <param name="tMatrix">T matrix.</param>
        /// <returns>Evaluated position, velocity, acceleration or jolt on the curve.</returns>
        [PublicAPI]
        public static Vector3 Evaluate(Matrix4x4 catmullRomCharacteristicMatrix, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            return pointMatrix * catmullRomCharacteristicMatrix * tMatrix * HALF;
        }

        /// <summary>
        /// Given a Catmull-Rom curve, return an interpolated position at ratio t using Bernstein polynomials.
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
                p1 * (2f * ttt - 5f * tt + 4f * t - 1f) +
                p2 * (-ttt + t) +
                p3 * (2f * tt);
        }
    }
}
