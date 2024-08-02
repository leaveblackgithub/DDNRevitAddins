using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class CategoriesExtension
    {
        public static Category ExtGetCategory(this Categories categories, BuiltInCategory builtInCategory)
        {
            return categories.get_Item(builtInCategory);
        }

        public static Category ExtNewSubCategory(this Categories categories, Category parentCategory, string name)
        {
            return categories.NewSubcategory(parentCategory, name);
        }
        public const BuiltInCategory BuiltInCategoryOfLines = BuiltInCategory.OST_Lines;
    }
}
