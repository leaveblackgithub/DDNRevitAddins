using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace DDNRevitAddins.AddinLibOfFamlyParameterTransfer
{
    public class FamilyInstanceSelectionFilter : ISelectionFilter
    {
        Document doc = null;
        public FamilyInstanceSelectionFilter(Document document)
        {
            doc = document;
        }

        public bool AllowElement(Element element)
        {
            if (!(element is FamilyInstance)) return false;
            return (!(element is ElementType));
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            Element element = doc.GetElement(refer);
            if (!(element is FamilyInstance)) return false;
            return (!(element is ElementType));
        }
    }
}