using UnityEngine;

namespace Varneon.VUdon.Splines
{
    public static class SplineUtility
    {
        public static Vector3 EvaluatePosition(SplineType type, Matrix4x4 pointMatrix, Vector4 tMatrix)
        {
            switch (type)
            {
                case SplineType.Linear:
                    return Vector3.zero;
                case SplineType.Bezier:
                    return BezierUtility.Evaluate(SplineCharacteristicMatrixFactory.CubicBezierMatrix(), pointMatrix, tMatrix);
                case SplineType.Hermite:
                    return HermiteSplineUtility.EvaluatePositionCached(SplineCharacteristicMatrixFactory.HermiteMatrix(), pointMatrix, tMatrix);
                case SplineType.CatmullRom:
                    return CatmullRomUtility.EvaluatePositionCached(SplineCharacteristicMatrixFactory.CatmullRomMatrix(), pointMatrix, tMatrix);
                case SplineType.BSpline:
                    return BSplineUtility.EvaluatePositionCached(SplineCharacteristicMatrixFactory.BSplineMatrix(), pointMatrix, tMatrix);
                default:
                    return Vector3.zero;
            }
        }

        public static Vector3 EvaluatePosition(SplineType type, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            switch (type)
            {
                case SplineType.Linear:
                    return Vector3.zero;
                case SplineType.Bezier:
                    return BezierUtility.EvaluatePositionBernstein(p0, p1, p2, p3, t);
                case SplineType.Hermite:
                    return HermiteSplineUtility.EvaluatePositionBernstein(p0, p1, p2, p3, t);
                case SplineType.CatmullRom:
                    return CatmullRomUtility.EvaluatePositionBernstein(p0, p1, p2, p3, t);
                case SplineType.BSpline:
                    return BSplineUtility.EvaluatePositionBernstein(p0, p1, p2, p3, t);
                default:
                    return Vector3.zero;
            }
        }
    }
}
