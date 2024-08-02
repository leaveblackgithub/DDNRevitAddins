using System.Collections.Generic;
using Autodesk.Revit.DB;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Geometries;
using Document = Autodesk.Revit.Creation.Document;

namespace DDNRevitAddins.Archive.Environments
{
    public static class CreateDocumentExtension
    {
        public static ModelCurve ExtNewModelCurve(this Document createDocument, Curve curve, SketchPlane sketchPlane)
        {
            return createDocument.NewModelCurve(curve, sketchPlane);
        }

        public static ModelCurve ExtNewModelLine(this Document createDocument, XYZ point1,XYZ point2, SketchPlane sketchPlane)
        {
            Curve curve = LineExtension.ExtCreateBound(point1.ExtProjectTo(sketchPlane), point2.ExtProjectTo(sketchPlane));
            return createDocument.NewModelCurve(curve, sketchPlane);
        }

        public static void ExtShowViewBoundingbox(this Document createDocument, View view)
        {
            BoundingBoxXYZ boundingBoxXyz = view.ExtCropBox();
            SketchPlane sketchPlane = view.ExtSketchPlane();
            DebugExtension.PrintFieldValue(nameof(ExtShowViewBoundingbox),nameof(BoundingBoxXyzExtension.ExtMin),boundingBoxXyz.ExtMin().ToString());
            XYZ min = boundingBoxXyz.ExtMinTransform();
            DebugExtension.PrintFieldValue(nameof(ExtShowViewBoundingbox), nameof(BoundingBoxXyzExtension.ExtMinTransform), boundingBoxXyz.ExtMinTransform().ToString());
            XYZ max = boundingBoxXyz.ExtMaxTransform();
            createDocument.ExtNewModelLine(min, max,sketchPlane);
        }

        public static Group ExtNewGroup(this Document createDocument, IList<ElementId> ids)
        {
            return createDocument.NewGroup(ids);
        }

        public static Dimension ExtNewDimensionFrRefArray(this Document createDocument, 
            View view,
            Line line,
            ReferenceArray references
            )
        {
            return createDocument.NewDimension(view, line, references);
        }
        public static Dimension ExtNewDimensionFrRefList(this Document createDocument,
            View view,
            Line line,
            IList<Reference> references
        )
        {
            ReferenceArray referenceArray = new ReferenceArray();
            foreach (Reference reference in references)
            {
                referenceArray.Append(reference);
            }

            return createDocument.ExtNewDimensionFrRefArray(view, line, referenceArray);
        }
    }
}