using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Castle.Core.Internal;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Environments;
using DDNRevitAddins.General;

namespace DDNRevitAddins.AddinLibOfFamlyParameterTransfer
{
    public abstract class FamilyParameterBase : ExternalCommandFamilyBase
    {
        internal Document RevitDbDocumentOfSource;
        internal List<FamilyParameter> SourceParameters;
        internal Dictionary<string, ExternalDefinition> ExternalDefinitions;
        internal FamilyManager RevitFamilyManagerOfSource;
        internal FrmParameters FrmParameters;

        internal override bool InitiateRevitAccessor(ExternalCommandData commandData)
        {
            if (!base.InitiateRevitAccessor(commandData)) return false;
            if (!InitialFamilyDocumentsAndParameters()) return false;
            return true;
        }

        internal bool InitialFamilyDocumentsAndParameters()
        {
            if (RevitFamilyManager.Types.Size == 0)
            {
                throw new ExceptionToCancel("Current Family must have type");
            }
            DebugExtension.PrintFieldValue(nameof(FamilyParameterTransfer), nameof(FamilyParameterTransfer.RunBeforeTransaction),
                "start");
            FrmParameters=new FrmParameters(RevitUiApplication);
            FrmParameters.GetOtherFamilyDocuments(GetOtherFamilyDocuments());
            if (FrmParameters.ShowDialog() != DialogResult.OK) return false;

            RevitDbDocumentOfSource = FrmParameters.SelectedDoc.Value;
            Dictionary<string, FamilyParameter> selectedParameters = FrmParameters.SelectedParameters;
            RevitFamilyManagerOfSource = RevitDbDocumentOfSource.ExtGetFamilyManager();

            if (RevitFamilyManagerOfSource.Types.Size == 0)
            {
                throw new ExceptionToCancel("Source Family must have type");
            }

            SourceParameters = selectedParameters.Values.ToList<FamilyParameter>();
            return true;
        }

        internal bool TransferParameter(FamilyParameter sourceParameter)
        {
            ExternalDefinition edefSource = RevitFamilyManagerOfSource .ExtGetExternalDefinitionOfShareParameter( sourceParameter, ExternalDefinitions);
            FamilyParameter targetParameter =RevitFamilyManager.ExtGetParameter(sourceParameter.Definition.Name);
            if (targetParameter != null)
            {
                ExternalDefinition edefTarget = RevitFamilyManager .ExtGetExternalDefinitionOfShareParameter( targetParameter, ExternalDefinitions);
                if(edefTarget==null & edefSource!=null)
                    RevitFamilyManager.ReplaceParameter(targetParameter,
                        edefSource,
                        sourceParameter.Definition.ParameterGroup,
                        sourceParameter.IsInstance);
            }
            else   
            { 
                if(edefSource==null)
                    RevitFamilyManager.AddParameter(sourceParameter.Definition.Name, 
                        sourceParameter.Definition.ParameterGroup, 
                        sourceParameter.Definition.ParameterType, 
                        sourceParameter.IsInstance);
                else
                    RevitFamilyManager.AddParameter(edefSource ,
                        sourceParameter.Definition.ParameterGroup,
                        sourceParameter.IsInstance);
            }
            return true;
        }

        internal void TransferParameters()
        {
            foreach (FamilyParameter parameter in SourceParameters)
            {
                TransferParameter(parameter);
            }
        }
    }
}