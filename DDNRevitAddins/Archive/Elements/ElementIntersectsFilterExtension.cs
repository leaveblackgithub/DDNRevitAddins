using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Elements
{
    public static class ElementIntersectsFilterExtension
    {
        public static ElementIntersectsElementFilter ExtNewElementIntersectsElementFilter(this Element element)
        {
            return new ElementIntersectsElementFilter(element);
        }

        public static bool ExtPassesFilter(this ElementIntersectsFilter filter, Element element)
        {
            return filter.PassesFilter(element);
        }
    }
}
