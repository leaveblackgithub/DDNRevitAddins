using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class ArcExtension
    {
        public static Arc ExtCreateCircle(XYZ ptCen, double radius)
        {
            return Arc.Create(PlaneExtension.ExtCreateByNormalAndOrigin(XYZ.BasisZ, ptCen), radius, 0, 2 * Math.PI);
            ;
        }
        public static Arc ExtCreate3Pt(XYZ pt1,XYZ pt2,XYZ pt3)
        {
            return Arc.Create(pt1, pt2, pt3);
        }
    }
}
