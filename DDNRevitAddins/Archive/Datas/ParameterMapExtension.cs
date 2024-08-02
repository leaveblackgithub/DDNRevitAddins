using System;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class ParameterMapExtension
    {
        private static bool ExtContains(this ParameterMap parameterMap, string parameterName)
        {
            return (parameterMap.Contains(parameterName));
        }

        public static Parameter ExtGetItem(this ParameterMap parameterMap, string parameterName)
        {
            if (!parameterMap.ExtContains(parameterName)) throw new ArgumentNullException();
            return parameterMap.get_Item(parameterName);
        }
    }
}
