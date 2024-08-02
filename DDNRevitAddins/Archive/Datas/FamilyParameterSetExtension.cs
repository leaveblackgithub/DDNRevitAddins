using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class FamilyParameterSetExtension
    {
        public static Dictionary<string, FamilyParameter> ExtToDictionary(this FamilyParameterSet familyParameterSet)
        {
           return  familyParameterSet.Cast<FamilyParameter>()
                .ToDictionary(p => p.ExtGetDefinitionName());
        }
    }
}
