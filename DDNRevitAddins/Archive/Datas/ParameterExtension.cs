using System;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class ParameterExtension
    {
        private static StorageType ExtStorageType(this Parameter parameter)
        {
            return parameter.StorageType;
        }

        public static string ExtAsString(this Parameter parameter)
        {
            if (parameter.ExtStorageType() != StorageType.String) throw new ArgumentNullException();
            return parameter.AsString();
        }

        public static double  ExtAsDouble(this Parameter parameter)
        {
            if (parameter.ExtStorageType() != StorageType.Double) throw new ArgumentNullException();
            return parameter.AsDouble();
        }

        public static bool ExtSet(this Parameter parameter, double value)
        {
            if (parameter.ExtStorageType() != StorageType.Double) throw new ArgumentNullException();
            return parameter.Set(value);

        }
        public static bool ExtSet(this Parameter parameter, string value)
        {
            if (parameter.ExtStorageType() != StorageType.String) throw new ArgumentNullException();
            return parameter.Set(value);

        }
        public static Definition ExtGetDefinition(this Parameter parameter)
        {
            return parameter.Definition;
        }
        public static string ExtGetDefinitionName(this Parameter parameter)
        {
            return parameter.ExtGetDefinition().ExtGetName();
        }
        public static ParameterType ExtGetParameterType(this Parameter parameter)
        {
            return parameter.ExtGetDefinition().ExtGetParameterType();
        }
    }
}
