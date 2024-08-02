using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNRevitAddins.Archive.Elements
{
    public static class SolidSolidCutUtilExtension
    {
        public static void ExtSplitFacesOfCuttingSolid(this Element first, Element second, bool split)
        {
            SolidSolidCutUtils.SplitFacesOfCuttingSolid(first,second,split);
        }
    }
}
