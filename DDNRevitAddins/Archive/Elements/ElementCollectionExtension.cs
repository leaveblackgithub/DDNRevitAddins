using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Elements
{
    public static class ElementCollectionExtension
    {
        public static IList<ElementId> ExtConvertToIds(this ICollection<Element> elements)
        {
            return elements.Select(e => e.Id).ToList();
        }
    }
}
