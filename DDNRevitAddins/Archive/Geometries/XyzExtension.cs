using Autodesk.Revit.DB;
using DDNGeneralLibrary;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class XyzExtension
    {
        public static XYZ ExtNewXyz(double x, double y, double z)
        {
            return new XYZ(x, y, z);
        }

        public static XYZ ExtGetMidPointTo(this XYZ point1, XYZ point2)
        {
            return ExtNewXyz(point1.X.ExtGetAverageTo(point2.X), point1.Y.ExtGetAverageTo(point2.Y),
                point1.Z.ExtGetAverageTo(point2.Z));
        }

        public static XYZ ExtAdd(this XYZ point1, XYZ point2)
        {
            return point1.Add(point2);
        }

        public static XYZ ExtSubtract(this XYZ point1, XYZ point2)
        {
            return point1.Subtract(point2);
        }
        public static XYZ ExtMultiply(this XYZ point1, double length)
        {
            return point1.Multiply(length);
        }
        public static XYZ ExtDivide(this XYZ point1, double length)
        {
            return point1.Divide(length);
        }

        public static XYZ ExtCrossProduct(this XYZ point1, XYZ point2)
        {
            return point1.CrossProduct(point2);
        }

        public static XYZ ExtNormalize(this XYZ vector1)
        {
            return vector1.Normalize();
        }
        public static double ExtDistanceTo(this XYZ point1, XYZ point2)
        {
            return point1.DistanceTo(point2);
        }

        public static double ExtAngleOnXyPlaneTo(this XYZ point1, XYZ point2)
        {
            return point1.AngleOnPlaneTo(point2, ExtBasisZ());
        }
        public static double ExtAngleOnPlaneTo(this XYZ point1, XYZ point2, XYZ normal)
        {
            return point1.AngleOnPlaneTo(point2, normal);
        }
        public static XYZ ExtBasisX()
        {
            return XYZ.BasisX;
        }
        public static XYZ ExtBasisY()
        {
            return XYZ.BasisY;
        }

        public static XYZ ExtBasisZ()
        {
            return XYZ.BasisZ;
        }

        public static XYZ ExtProjectTo(this XYZ point, SketchPlane sketchPlane)
        {
            XYZ xyz = sketchPlane.ExtOrigin();
            double z = xyz.Z;
            return XyzExtension.ExtNewXyz(point.X, point.Y, z);
        }

        public static double ExtGetLength(this XYZ vector)
        {
            return vector.GetLength();
        }
    }
}