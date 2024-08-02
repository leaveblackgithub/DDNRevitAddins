using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class TransformExtension
    {
        public static XYZ ExtOfPoint(this Transform transform,XYZ point)
        {
            return transform.OfPoint(point);
        }
    }
}
