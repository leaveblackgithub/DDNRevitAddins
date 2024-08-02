using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using DDNRevitAddins.Archive.Elements;

namespace DDNRevitAddins.Archive.Datas
{
    public class CategoryNameSelectionFilter : ISelectionFilter
    {
        protected string CategoryName;

        public CategoryNameSelectionFilter( string categoryName)
        {
            CategoryName = categoryName;
        }
        public bool AllowElement(Element elem)
        {
           
            return elem.ExtCategoryName() == CategoryName;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}