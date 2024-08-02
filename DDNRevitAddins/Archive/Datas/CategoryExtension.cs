using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static  class CategoryExtension
    {
        public static CategoryNameMap ExtGetSubCategories(this Category category)
        {
            return category.SubCategories;
        }
        public static GraphicsStyle ExtGetGraphicStyle(this Category category,GraphicsStyleType graphicsStyleType)
        {
            return category.GetGraphicsStyle(graphicsStyleType);
        }
        public static GraphicsStyle ExtGetGraphicStyleOfProjection(this Category category)
        {
            return category.ExtGetGraphicStyle(GraphicsStyleType.Projection);
        }

        public static Category ExtGetSubCategory(this Category category, string name)
        {
            return category.ExtGetSubCategories().ExtGetCategory(name);
        }
    }
}
