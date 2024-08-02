using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class LocationExtension
    {
        public static bool ExtRotate(this Location location,Line axis,double angle)
        {
            return location.Rotate(axis, angle);
        }

        public static bool ExtRotateOnXyPlane(this Location location, XYZ origin, double angle)
        {
            Line axis = LineExtension.ExtCreateUnBound(origin, XyzExtension.ExtBasisZ());
            return location.ExtRotate(axis, angle);
        }
    }
}
