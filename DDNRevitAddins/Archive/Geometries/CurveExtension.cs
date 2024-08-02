using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class CurveExtension
    {
        public static XYZ ExtEvaluate(this Curve curve,double parameter, bool normalized)
        {
            return curve.Evaluate(parameter, normalized);
        }
        public static XYZ ExtGetMidPoint(this Curve curve)
        {
            return curve.Evaluate(0.5, true);
        }
    }
}
