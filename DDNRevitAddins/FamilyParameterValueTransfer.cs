using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DDNGeneralLibrary;
using DDNRevitAddins.AddinLibOfFamlyParameterTransfer;
using DDNRevitAddins.Archive.Environments;

namespace DDNRevitAddins
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class FamilyParameterValueTransfer : FamilyParameterBase
    {
        private Dictionary<FamilyParameter, object> _parameterValueDictionary;
        private FamilyType _sourceType;

        protected override bool RunBeforeTransaction()
        {

            _parameterValueDictionary = new Dictionary<FamilyParameter, object>();

            _sourceType = RevitFamilyManagerOfSource.CurrentType;

            if (!GetParameterValues()) return false;
            AddSingleTransactionHandler((SingleTransactionHandler)TransferParameterValues);
            return true;
        }

        private bool GetParameterValues()
        {
            foreach (FamilyParameter sourceParameter in SourceParameters)
            {
                string name = sourceParameter.Definition.Name;
                FamilyParameter targetParameter = RevitFamilyManager.get_Parameter(name);
               
                GetParameterValue(sourceParameter, targetParameter);
            }
            if (_parameterValueDictionary.Count == 0) return false;
            return true;
        }

        private void GetParameterValue(FamilyParameter sourceParameter, FamilyParameter targetParameter)
        {
            string definitionName = sourceParameter.Definition.Name;
            if (targetParameter == null)
            {
                TaskDialogExtension.ExtShow(nameof(FamilyParameterValueTransfer),
                    "Transfer parameter ["+definitionName +"] first.");
                return;
            }

            if (targetParameter.IsReadOnly)
            {
                TaskDialogExtension.ExtShow(nameof(FamilyParameterValueTransfer),
                    "Target parameter [" + definitionName + "] is readonly.");
                return;
            }
            if (sourceParameter.IsDeterminedByFormula)
            {
                TaskDialogExtension.ExtShow(nameof(FamilyParameterValueTransfer),
                    "Source parameter [" + definitionName + "] is formula-driven.");
                return;
            }
            if (targetParameter.IsDeterminedByFormula)
            {
                TaskDialogExtension.ExtShow(nameof(FamilyParameterValueTransfer),
                    "Target parameter [" + definitionName + "] is formula-driven.");
                return;
            }
            if ((targetParameter.StorageType != sourceParameter.StorageType) ||
                (targetParameter.Definition.ParameterType != sourceParameter.Definition.ParameterType))
            {
                TaskDialogExtension.ExtShow(nameof(FamilyParameterValueTransfer),
                    "Source and Target parameter [" + definitionName + "] are not same type.");
                return;
            }

            if (_sourceType.HasValue(sourceParameter) == false)
            {
                TaskDialogExtension.ExtShow(nameof(FamilyParameterValueTransfer),
                    "Source parameter [" + definitionName + "] does not have value");
                return;
            }
            switch (sourceParameter.StorageType)
            {
                case StorageType.Double:
                    _parameterValueDictionary.Add(targetParameter, _sourceType.AsDouble(sourceParameter));
                    break;
                case StorageType.Integer:
                    _parameterValueDictionary.Add(targetParameter, _sourceType.AsInteger(sourceParameter));
                    break;
                case StorageType.String:
                    _parameterValueDictionary.Add(targetParameter, _sourceType.AsString(sourceParameter));
                    break;
                default:
                    break;
            }
        }

        protected void TransferParameterValues()
        {
            foreach (KeyValuePair<FamilyParameter, object> pair in _parameterValueDictionary)
            {
                FamilyParameter parameter = pair.Key;
                object value = pair.Value;
                switch (parameter.StorageType)
                {
                    case StorageType.Double:
                        RevitFamilyManager.Set(parameter, (double)value);
                        break;
                    case StorageType.Integer:
                        RevitFamilyManager.Set(parameter, (int)value);
                        break;
                    case StorageType.String:
                        RevitFamilyManager.Set(parameter, (string)value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
