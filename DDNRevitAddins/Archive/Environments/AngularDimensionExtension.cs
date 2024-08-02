using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Environments
{
    public static class AngularDimensionExtension
    {
        public static Dimension ExtCreate(Autodesk.Revit.DB.Document document,
            View dbView,
            Arc arc,
            IList<Reference> references,
            DimensionType dimensionStyle
        )
        {
            return AngularDimension.Create(document, dbView, arc, references, dimensionStyle);
        }
        public static Dimension ExtCreateDefaultType(Autodesk.Revit.DB.Document document,
            View dbView,
            Arc arc,
            IList<Reference> references
        )
        {
            return AngularDimension.Create(document, dbView, arc, references, document.ExtGetDefaultDimensionType());
        }
    }
}
