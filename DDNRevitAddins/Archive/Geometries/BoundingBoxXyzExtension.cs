using Autodesk.Revit.DB;
using DDNGeneralLibrary;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class BoundingBoxXyzExtension
    {
        public static XYZ ExtMin(this BoundingBoxXYZ boundingBoxXyz )
        {
            return boundingBoxXyz.Min;
        }
        public static XYZ ExtMinTransform(this BoundingBoxXYZ boundingBoxXyz)
        {
            XYZ min = boundingBoxXyz.ExtMin();
            XYZ minPoint = boundingBoxXyz.ExtTransform().ExtOfPoint(min);
            return minPoint;
        }

        public static XYZ ExtMax(this BoundingBoxXYZ boundingBoxXyz)
        {
            return boundingBoxXyz.Max;
        }

        public static XYZ ExtMaxTransform(this BoundingBoxXYZ boundingBoxXyz)
        {
            XYZ max = boundingBoxXyz.ExtMax();
            XYZ maxPoint = boundingBoxXyz.ExtTransform().ExtOfPoint(max);
            return maxPoint;
        }
        public static XYZ ExtCenter(this BoundingBoxXYZ boundingBoxXyz)
        {
            return boundingBoxXyz.ExtMin().ExtGetMidPointTo(boundingBoxXyz.ExtMax());
        }

        public static Transform ExtTransform(this BoundingBoxXYZ boundingBoxXyz)
        {
            return boundingBoxXyz.Transform;
        }

        public static double ExtRadian(this BoundingBoxXYZ boundingBoxXyz)
        {
            XYZ cen = boundingBoxXyz.ExtCenter();
            XYZ min = boundingBoxXyz.ExtMin();
            XYZ minTransform = boundingBoxXyz.ExtMinTransform();
            XYZ direction = min.ExtSubtract(cen);
            XYZ directionTransform = minTransform.ExtSubtract(cen);
            return direction.ExtAngleOnXyPlaneTo(directionTransform);
        }

        public static double ExtAngle(this BoundingBoxXYZ boundingBoxXyz)
        {
            return boundingBoxXyz.ExtRadian().ExtRadianToAngle();
        }
    }
}
