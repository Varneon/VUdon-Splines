using UnityEngine;

namespace Varneon.VUdon.Splines
{
    /// <summary>
    /// Types of splines
    /// </summary>
    public enum SplineType
    {
        Linear,

        [InspectorName("Bézier")]
        Bezier,

        Hermite,

        [InspectorName("Catmull-Rom")]
        CatmullRom,

        [InspectorName("B-Spline")]
        BSpline
    }
}
