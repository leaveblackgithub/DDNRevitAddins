using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Environments
{
    public static class DbDocumentSetExtension
    {
        public static IEnumerable<Document> ExtCast(this DocumentSet documentSet)
        {
            return documentSet.Cast<Document>();
        }
        public static IEnumerable<Document> ExtGetFamilyDocuments(this IEnumerable<Document> documents)
        {
            return documents.Where(p => p.ExtIsFamilyDocument());
        }
        public static IEnumerable<Document> ExtExcludeActiveDocument(this IEnumerable<Document> documents,Document currentDocument)
        {
            return documents.Where(p =>! p .ExtIsSameDocument(currentDocument));
        }
    }
}
