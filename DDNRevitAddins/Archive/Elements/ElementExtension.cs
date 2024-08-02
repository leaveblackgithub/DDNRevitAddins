using System;
using Autodesk.Revit.DB;
using DDNRevitAddins.Archive.Datas;
using DDNRevitAddins.Archive.Environments;
using DDNRevitAddins.Archive.Geometries;

namespace DDNRevitAddins.Archive.Elements
{
    public static class ElementExtension
    {
        public static string ExtCategoryName(this Element element)
        {
            Category category = element.Category;
            if (category == null) return "";
            return category.Name;
        }

        public static int ExtIdValue(this Element element)
        {
            return element.ExtId().ExtIntegerValue();
        }

        public static ElementId ExtId(this Element element)
        {
            return element.Id;
        }

        public static void ExtDelete(this Element element)
        {
            element.ExtDbDocument().ExtDelete(element.ExtId());
        }
        public static BoundingBoxXYZ ExtBoundingBox(this Element element)
        {
            return element.get_BoundingBox(element.ExtOwnerView());
        }
        
        public static View ExtOwnerView(this Element element)
        {
            return element.ExtDbDocument().ExtGetElement(element.ExtOwnerViewId()) as View;
        }

        public static ElementId ExtOwnerViewId(this Element element)
        {
            return element.OwnerViewId;
        }

        public static Document ExtDbDocument(this Element element)
        {
            return element.Document;
        }

        public static string ExtGetTypeParameterValueString(this Element element, string parameterName)
        {
            return element.ExtGetTypeElement().ExtGetParameterValueString(parameterName);
        }

        private static Element ExtGetTypeElement(this Element element)
        {
            return element.ExtDbDocument().ExtGetElement(element.ExtGetTypeId());
        }
        private static ElementId ExtGetTypeId(this Element element)
        {
            ElementId typeId = element.GetTypeId();
            if (typeId == ElementId.InvalidElementId) throw new ArgumentNullException();
            return typeId;
        }

        public static double ExtGetParameterValueDouble(this Element element, string parameterName)
        {
            return element.ExtGetParameter(parameterName).ExtAsDouble();
        }
        public static bool ExtSetParameterValueDouble(this Element element, string parameterName,double value)
        {
            return element.ExtGetParameter(parameterName).ExtSet(value);
        }
        public static bool ExtSetParameterValueString(this Element element, string parameterName, string value)
        {
            return element.ExtGetParameter(parameterName).ExtSet(value);
        }


        public static string ExtGetParameterValueString(this Element element, string parameterName)
        {
            return element.ExtGetParameter(parameterName).ExtAsString();
        }


        private static Parameter  ExtGetParameter(this Element element, string parameterName)
        {
            return element.ExtParametersMap().ExtGetItem(parameterName);
        }

        private static ParameterMap ExtParametersMap(this Element element)
        {
            return element.ParametersMap;
        }

        private static Location ExtLocation(this Element element)
        {
            return element.Location;
        }

        public static bool ExtRotate(this Element element, Line axis, double angle)
        {
            return element.ExtLocation().ExtRotate(axis, angle);
        }

        public static bool ExtRotateOnXyPlane(this Element element, XYZ origin, double angle)
        {
            return element.ExtLocation().ExtRotateOnXyPlane(origin, angle);
        }

        public static string ExtGetName(this Element element)
        {
            return element.Name;
        }
        public static void ExtSetName(this Element element, string newName)
        {
            element.Name = newName;
        }
        public static void ExtGetGroup(this Element element, string newName)
        {
            element.Name = newName;
        }

        public static GeometryObject ExtGetGeoRef(this Element element, Reference reference)
        {
            return element.GetGeometryObjectFromReference(reference);
        }

        public const string CategoryNameOfViews = "CategoryNameOfViews";
        public const string CategoryNameOfRasterImages = "Raster Images";
        public const string CategoryNameOfCurtainPanels = "Curtain Panels";
        public static string CategoryNameOfGenericModels = "Generic Models";
    }
}
