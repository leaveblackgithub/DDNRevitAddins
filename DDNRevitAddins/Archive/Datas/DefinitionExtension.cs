using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class DefinitionExtension
    {
        public static string ExtGetName(this Definition definition)
        {
            return definition.Name;
        }
        public static ParameterType ExtGetParameterType(this Definition definition)
        {
            return definition.ParameterType;
        }
    }
}
