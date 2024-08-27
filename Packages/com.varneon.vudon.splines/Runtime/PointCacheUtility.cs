using System;
using System.Collections.Generic;
using UnityEngine;

namespace Varneon.VUdon.Splines
{
    public static class PointCacheUtility
    {
        public static void RebuildPointCache(ref Matrix4x4[] pointCache, UdonSplineKnotBase[] knots, SplineType type)
        {
            Array.Resize(ref pointCache, knots.Length);

            switch (type)
            {
                case SplineType.Bezier:
                    RebuildBezierPointCache(ref pointCache, knots);
                    break;
                case SplineType.Hermite:
                    RebuildHermitePointCache(ref pointCache, knots);
                    break;
                case SplineType.CatmullRom:
                    RebuildCatmullRomPointCache(ref pointCache, knots);
                    break;
                case SplineType.BSpline:
                    RebuildBSplinePointCache(ref pointCache, knots);
                    break;
            }
        }

        public static void RebuildBezierPointCache(ref Matrix4x4[] pointCache, UdonSplineKnotBase[] knots)
        {
            int count = knots.Length;

            for (int i = 0; i < count; i++)
            {
                UdonSplineKnotBase k0 = knots[i % count];
                UdonSplineKnotBase k1 = knots[(i + 1) % count];

                Vector3 p0 = k0.Position;
                Vector3 p3 = k1.Position;

                pointCache[i] = new Matrix4x4(p0, p0 + k0.Rotation * k0.TangentOut, p3 + k1.Rotation * k1.TangentIn, p3);
            }
        }

        public static void RebuildHermitePointCache(ref Matrix4x4[] pointCache, UdonSplineKnotBase[] knots)
        {
            int count = knots.Length;

            for (int i = 0; i < count; i++)
            {
                UdonSplineKnotBase k0 = knots[i % count];
                UdonSplineKnotBase k1 = knots[(i + 1) % count];

                pointCache[i] = new Matrix4x4(k0.Position, k0.Rotation * k0.TangentOut * 3f, k1.Position, k1.Rotation * k1.TangentOut * 3f);
            }
        }

        public static void RebuildCatmullRomPointCache(ref Matrix4x4[] pointCache, UdonSplineKnotBase[] knots)
        {
            int count = knots.Length;

            for (int i = 0; i < count; i++)
            {
                UdonSplineKnotBase k0 = knots[i % count];
                UdonSplineKnotBase k1 = knots[(i + 1) % count];
                UdonSplineKnotBase k2 = knots[(i + 2) % count];
                UdonSplineKnotBase k3 = knots[(i + 3) % count];

                pointCache[i] = new Matrix4x4(k0.Position, k1.Position, k2.Position, k3.Position);
            }
        }

        public static void RebuildBSplinePointCache(ref Matrix4x4[] pointCache, UdonSplineKnotBase[] knots)
        {
            int count = knots.Length;

            for (int i = 0; i < count; i++)
            {
                pointCache[i] = new Matrix4x4(knots[i % count].Position, knots[(i + 1) % count].Position, knots[(i + 2) % count].Position, knots[(i + 3) % count].Position);
            }
        }

        public static void RebuildPointCache(ref Matrix4x4[] pointCache, List<UdonSplineStorage.Knot> knots, SplineType type)
        {
            Array.Resize(ref pointCache, knots.Count);

            switch (type)
            {
                case SplineType.Bezier:
                    RebuildBezierPointCache(ref pointCache, knots);
                    break;
                case SplineType.Hermite:
                    RebuildHermitePointCache(ref pointCache, knots);
                    break;
                case SplineType.CatmullRom:
                    RebuildCatmullRomPointCache(ref pointCache, knots);
                    break;
                case SplineType.BSpline:
                    RebuildBSplinePointCache(ref pointCache, knots);
                    break;
            }
        }

        public static void RebuildBezierPointCache(ref Matrix4x4[] pointCache, List<UdonSplineStorage.Knot> knots)
        {
            int count = knots.Count;

            for (int i = 0; i < count; i++)
            {
                UdonSplineStorage.Knot k0 = knots[i % count];
                UdonSplineStorage.Knot k1 = knots[(i + 1) % count];

                Vector3 p0 = k0.Position;
                Vector3 p3 = k1.Position;

                pointCache[i] = new Matrix4x4(p0, p0 + Quaternion.Euler(k0.Rotation) * k0.TangentOut, p3 + Quaternion.Euler(k1.Rotation) * k1.TangentIn, p3);
            }
        }

        public static void RebuildHermitePointCache(ref Matrix4x4[] pointCache, List<UdonSplineStorage.Knot> knots)
        {
            int count = knots.Count;

            for (int i = 0; i < count; i++)
            {
                UdonSplineStorage.Knot k0 = knots[i % count];
                UdonSplineStorage.Knot k1 = knots[(i + 1) % count];

                pointCache[i] = new Matrix4x4(k0.Position, Quaternion.Euler(k0.Rotation) * Vector3.forward * k0.Velocity, k1.Position, Quaternion.Euler(k1.Rotation) * Vector3.forward * k1.Velocity);
            }
        }

        public static void RebuildCatmullRomPointCache(ref Matrix4x4[] pointCache, List<UdonSplineStorage.Knot> knots)
        {
            int count = knots.Count;

            for (int i = 0; i < count; i++)
            {
                UdonSplineStorage.Knot k0 = knots[i % count];
                UdonSplineStorage.Knot k1 = knots[(i + 1) % count];
                UdonSplineStorage.Knot k2 = knots[(i + 2) % count];
                UdonSplineStorage.Knot k3 = knots[(i + 3) % count];

                pointCache[i] = new Matrix4x4(k0.Position, k1.Position, k2.Position, k3.Position);
            }
        }

        public static void RebuildBSplinePointCache(ref Matrix4x4[] pointCache, List<UdonSplineStorage.Knot> knots)
        {
            int count = knots.Count;

            for (int i = 0; i < count; i++)
            {
                pointCache[i] = new Matrix4x4(knots[i % count].Position, knots[(i + 1) % count].Position, knots[(i + 2) % count].Position, knots[(i + 3) % count].Position);
            }
        }
    }
}
