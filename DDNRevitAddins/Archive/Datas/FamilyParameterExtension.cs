using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class FamilyParameterExtension
    {
        public static Definition ExtGetDefinition(this FamilyParameter familyParameter)
        {
            return familyParameter.Definition;
        }
        public static string ExtGetDefinitionName(this FamilyParameter familyParameter)
        {
            return familyParameter.ExtGetDefinition().ExtGetName();
        }
        public static ParameterType ExtGetParameterType(this FamilyParameter familyParameter)
        {
            return familyParameter.ExtGetDefinition().ExtGetParameterType();
        }
    }
}
