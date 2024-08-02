using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Datas
{
    public static class ElementIdExtension
    {
        public static int ExtIntegerValue(this ElementId id)
        {
            return id.IntegerValue;
        }

        public static ElementId ExtNewElementIdFrInt(int idValue)
        {
            return new ElementId(idValue);
        }

        public static ElementId ExtNewElementIdFrBuiltinCategory(BuiltInCategory builtInCategory)
        {
            return new ElementId(builtInCategory);
        }
    }
}
