using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using DDNGeneralLibrary;
using DDNRevitAddins.AddinLibOfFamlyParameterTransfer;
using DDNRevitAddins.Archive.Environments;

namespace DDNRevitAddins
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class FamilyParameterRemove : FamilyParameterBase
    {
        protected override bool RunBeforeTransaction()
        {
            return true;
        }

    }
}
