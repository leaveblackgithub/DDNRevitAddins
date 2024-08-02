using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class SketchPlaneExtension
    {
        public static Plane ExtGetPlane(this SketchPlane sketchPlane)
        {
            return sketchPlane.GetPlane();
        }
        public static XYZ ExtOrigin(this SketchPlane sketchPlane)
        {
            return sketchPlane.ExtGetPlane().ExtOrigin();
        }
    }
}
