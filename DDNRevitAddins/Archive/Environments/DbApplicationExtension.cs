using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DDNRevitAddins.Archive.Geometries;

namespace DDNRevitAddins.Archive.Environments
{
    public static class DbApplicationExtension
    {
        public static double ExtShortCurveTolerance(this Application application)
        {
            return application.ShortCurveTolerance;
        }
        public static bool ExtAlmostEqualTo(this Application application, XYZ point1, XYZ point2)
        {
            return point1.ExtDistanceTo(point2) <= application.ExtShortCurveTolerance();
        }

        public static DocumentSet ExtGetDocumentSet(this Application application)
        {
            return application.Documents;
        }
        public static IEnumerable<Document> ExtGetDocuments(this Application application)
        {
            return application.ExtGetDocumentSet().ExtCast();
        }

        public static Autodesk.Revit.Creation.Application ExtGetCreateApplication(this Application application)
        {
            return application.Create;
        }

        public static Document ExtNewFamilyDocument(this Application application, string templateFileName)
        {
            return application.NewFamilyDocument(templateFileName);
        }
    }
}
