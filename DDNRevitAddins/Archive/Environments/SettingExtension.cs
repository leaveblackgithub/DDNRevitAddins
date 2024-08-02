using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Environments
{
    public static class SettingExtension
    {
        public static Categories ExtGetCategories(this Settings settings)
        {
            return settings.Categories;
        }
    }
}
