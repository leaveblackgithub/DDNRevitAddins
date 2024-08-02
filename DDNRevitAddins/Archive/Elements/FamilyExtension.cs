﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Elements
{
    public static class FamilyExtension
    {
        public static ISet<ElementId> ExtGetFamilySymbolIds(this Family family)
        {
            return family.GetFamilySymbolIds();
        }
    }
}
