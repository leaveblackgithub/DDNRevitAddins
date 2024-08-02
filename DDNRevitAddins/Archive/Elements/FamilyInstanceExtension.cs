using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using DDNRevitAddins.Archive.Datas;

namespace DDNRevitAddins.Archive.Elements
{
    public static class FamilyInstanceExtension
    {
        public static IList <Parameter> ExtGetParameterList(this FamilyInstance instance)
        {
            return instance.ExtGetParameters().ExtToList();
        }

        private static ParameterSet ExtGetParameters(this FamilyInstance instance)
        {
            return instance.Parameters;
        }
    }
}
