using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Geometries
{
    public static class PlaneExtension
    {
        public static Plane ExtCreateByOriginAndBasis(XYZ origin, XYZ basisX, XYZ basisY)
        {
            return Plane.CreateByOriginAndBasis(origin, basisX, basisY);
        }
        public static Plane ExtCreateByNormalAndOrigin(XYZ normal, XYZ origin)
        {
            return Plane.CreateByNormalAndOrigin(normal, origin);
        }
        public static Plane ExtCreateXyPlaneByOrigin(XYZ origin)
        {
            return Plane.CreateByOriginAndBasis(origin, XyzExtension.ExtBasisX(), XyzExtension.ExtBasisY());
        }

        public static XYZ ExtOrigin(this Plane plane)
        {
            return plane.Origin;
        }
    }
}
