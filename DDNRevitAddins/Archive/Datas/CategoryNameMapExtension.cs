using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class CategoryNameMapExtension
    {
        public static bool ExtContains(this CategoryNameMap categoryNameMap, string name)
        {
            return categoryNameMap.Contains(name);
        }
        public static Category ExtGetCategory(this CategoryNameMap categoryNameMap, string name)
        {
            if (!categoryNameMap.Contains(name)) return null;
            return categoryNameMap.get_Item(name);
        }
    }
}
