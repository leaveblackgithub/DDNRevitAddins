using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using DDNRevitAddins.Archive.Datas;

namespace DDNRevitAddins.Archive.Environments
{
    public static class FamilyManagerExtension
    {
        public static Dictionary<string, FamilyParameter> ExtGetFamilyParameterDictionary(this FamilyManager familyManager)
        {
            return familyManager.ExtGetParameters() .ExtToDictionary();
        }

        private static FamilyParameterSet ExtGetParameters(this FamilyManager familyManager)
        {
            return familyManager.Parameters;
        }

        public static bool ExtCanElementParameterBeAssociated(this FamilyManager familyManager, Parameter parameter)
        {
            return familyManager.CanElementParameterBeAssociated(parameter);
        }
        public static void ExtAssociateElementParameterToFamilyParameter(this FamilyManager familyManager, Parameter parameter,FamilyParameter familyParameter)
        {
            familyManager.AssociateElementParameterToFamilyParameter(parameter,familyParameter);
        }

        public static Dictionary<string, FamilyParameter> ExtGetParameterDictionary(FamilyManager docFamilyManager)
        {
            FamilyParameterSet parameters = docFamilyManager.Parameters;
            Dictionary<string, BuiltInParameterGroup> parameterList = new Dictionary<string, BuiltInParameterGroup>();
            foreach (FamilyParameter parameter in parameters)
            {
                InternalDefinition idef = parameter.Definition as InternalDefinition;
                if (idef.BuiltInParameter == BuiltInParameter.INVALID)
                    parameterList.Add(parameter.Definition.Name, parameter.Definition.ParameterGroup);
            }

            Dictionary<string, FamilyParameter> parameterListNew = new Dictionary<string, FamilyParameter>();
            IOrderedEnumerable<KeyValuePair<string, BuiltInParameterGroup>> orderList = parameterList.OrderBy
                (parameter => parameter.Value).ThenBy(parameter => parameter.Key);
            foreach (KeyValuePair<string, BuiltInParameterGroup> item in orderList)
            {
                parameterListNew.Add(item.Key, docFamilyManager.get_Parameter(item.Key));
            }

            return parameterListNew;
        }

        public static FamilyParameter ExtGetShareParameter(this FamilyManager familyManager, Guid guid)
        {
            return familyManager.get_Parameter(guid);
        }
        public static FamilyParameter ExtGetParameter(this FamilyManager familyManager,string name)
        {
            return familyManager.get_Parameter(name);
        }

        public static ExternalDefinition ExtGetExternalDefinitionOfShareParameter(this FamilyManager familyManager,
            FamilyParameter familyParameter,
            Dictionary<string, ExternalDefinition> externalDefinitions)
        {
            string parameterName = familyParameter.ExtGetDefinitionName();
            if (externalDefinitions.ContainsKey(parameterName) == false) return null;
            ExternalDefinition edef = externalDefinitions[parameterName];
            if (familyManager.ExtGetShareParameter(edef.GUID) == null) return null;
            return edef;
        }
    }
}
