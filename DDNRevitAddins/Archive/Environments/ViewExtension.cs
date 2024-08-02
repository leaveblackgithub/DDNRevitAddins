using Autodesk.Revit.DB;
using DDNGeneralLibrary;

namespace DDNRevitAddins.Archive.Environments
{
    public static class ViewExtension
    {
        public static SketchPlane ExtSketchPlane(this View view)
        {
            return view.SketchPlane;
        }

        public static XYZ ExtUpDirection(this View view)
        {
            XYZ upDirection = view.UpDirection;
            DebugExtension.PrintFieldValue(nameof(ExtUpDirection),nameof(upDirection),upDirection.ToString());
            return upDirection;
        }

        public static BoundingBoxXYZ ExtCropBox(this View view)
        {
            return view.CropBox;
        }

        public static int ExtGetScale(this View view)
        {
            return view.Scale;
        }

        public static XYZ ExtGetOrigin(this View view)
        {
            return view.Origin;
        }
        public static XYZ ExtGetRightDirection(this View view)
        {
            return view.RightDirection;
        }
        public static XYZ ExtGetUpDirection(this View view)
        {
            return view.UpDirection;
        }


    }
}
