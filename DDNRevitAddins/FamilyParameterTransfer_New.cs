using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Castle.Core.Internal;
using DDNGeneralLibrary;
using DDNRevitAddins.AddinLibOfFamlyParameterTransfer;
using DDNRevitAddins.Archive.Environments;

namespace DDNRevitAddins
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class FamilyParameterTransferNew:ExternalCommandFamilyBase
    {
        private IDictionary<string, Document> _otherFamilyDocuments;

        protected override bool RunBeforeTransaction()
        {
            FormOfParameter formOfParameter = new FormOfParameter();
            _otherFamilyDocuments = GetOtherFamilyDocuments();
            formOfParameter.GetFamilyDocumentsToCopyFrom(_otherFamilyDocuments.Keys.ToList());
            formOfParameter.ShowDialog();
            return true;
        }
    }
}
