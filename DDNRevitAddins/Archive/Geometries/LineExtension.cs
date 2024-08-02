using System.Windows.Forms;
using Autodesk.Revit.DB;
using DDNRevitAddins.General;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class LineExtension
    {
        private const int LongLength = 50000;

        public static Autodesk.Revit.DB.Line ExtCreateBound(XYZ point1, XYZ point2)
        {
            return Line.CreateBound(point1, point2);
        }
        public static Autodesk.Revit.DB.Line ExtCreateBoundWithExtension(XYZ point1, XYZ point2)
        {
            XYZ vector1 = point2.ExtSubtract(point1).ExtMultiply(LongLength);
            XYZ vector2 = point1.ExtSubtract(point2).ExtMultiply(LongLength);
            return Line.CreateBound(point1.ExtAdd(vector1), point2.ExtAdd((vector2)));
        }
        public static Autodesk.Revit.DB.Line ExtCreateUnBound(XYZ point1, XYZ vector)
        {
            return Line.CreateUnbound(point1, vector);
        }

        public static XYZ ExtIntersectLine(this Line line1, Line line2)
        {
            var intersectionResultArray = new IntersectionResultArray();
            var intersectCompare = line1.Intersect(line2, out intersectionResultArray);
            if (intersectCompare != SetComparisonResult.Overlap || intersectionResultArray.Size != 1) return null;
            return intersectionResultArray.get_Item(0).XYZPoint;

        }
        public static XYZ ExtIntersectCurveCloseTo(this Line line1, Curve curve1,XYZ pt1)
        {
            var intersectionResultArray = new IntersectionResultArray();
            var intersectCompare = line1.Intersect(curve1, out intersectionResultArray);
            if (intersectCompare==SetComparisonResult.Disjoint) return null;
            int i = 0;
            XYZ ptResult=null;
            double distResult = 5000000000000;
            while (i < intersectionResultArray.Size)
            {
                XYZ ptIns = intersectionResultArray.get_Item(i).XYZPoint;
                double distIns = pt1.ExtDistanceTo(ptIns);
                if (distResult > distIns)
                {
                    distResult = distIns;
                    ptResult = ptIns;
                }

                i++;
            }
            return ptResult;

        }
    }
}
