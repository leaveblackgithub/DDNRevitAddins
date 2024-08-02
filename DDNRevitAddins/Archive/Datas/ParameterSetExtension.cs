using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class ParameterSetExtension
    {
        public static IList<Parameter> ExtToList(this ParameterSet parameterSet)
        {
            return parameterSet.Cast<Parameter>().ToList();
        }
    }
}
