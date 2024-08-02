using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Castle.Core.Internal;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Geometries;
using DDNRevitAddins.General;

namespace DDNRevitAddins.Archive.Environments
{
    public abstract class ExternalCommandFamilyBase : ExternalCommandBase
    {
        protected FamilyManager RevitFamilyManager;


        internal override bool InitiateRevitAccessor(ExternalCommandData commandData)
        {
            if (!base.InitiateRevitAccessor(commandData)) return false;
            ThrowExceptionIfNoFamilyDocument();
            RevitFamilyManager = RevitDbDocument.ExtGetFamilyManager();
            return true;
        }

        protected IDictionary<string, Document> GetOtherFamilyDocuments()
        {
            IDictionary<string, Document> result = RevitDbApplication.ExtGetDocuments().ExtGetFamilyDocuments().ExtExcludeActiveDocument(RevitDbDocument)
                .ToDictionary(p => p.ExtGetTitle());
            if (result.IsNullOrEmpty())
                throw new ExceptionToCancel("Can't find other family documents to copy from.");
            return result;
        }
    }
}