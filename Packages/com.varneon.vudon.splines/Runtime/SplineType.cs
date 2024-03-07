using UnityEngine;

namespace Varneon.VUdon.Splines
{
    /// <summary>
    /// Types of splines
    /// </summary>
    public enum SplineType
    {
        Linear,

        [InspectorName("B�zier")]
        Bezier,

        Hermite,

        [InspectorName("Catmull-Rom")]
        CatmullRom,

        [InspectorName("B-Spline")]
        BSpline
    }
}
