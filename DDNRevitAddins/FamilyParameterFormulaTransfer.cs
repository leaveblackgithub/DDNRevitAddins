using System;
using System.Collections;
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
    public class FamilyParameterFormulaTransfer : FamilyParameterBase
    {
        private Dictionary<FamilyParameter, FormulaInfo> formulaInfos;

        protected override bool RunBeforeTransaction()
        {
            formulaInfos = new Dictionary<FamilyParameter, FormulaInfo>();
            FamilyType type = RevitFamilyManager.CurrentType;
            foreach (FamilyParameter parameter in SourceParameters)
            {
                if (parameter.IsDeterminedByFormula == false) continue;

                string name = parameter.Definition.Name;
                FamilyParameter tpara = RevitFamilyManager.get_Parameter(name);
                if (tpara == null) continue;

                if (tpara.IsReadOnly) continue;
                if ((tpara.StorageType != parameter.StorageType) || (tpara.Definition.ParameterType != parameter.Definition.ParameterType)) continue;

                string formula = parameter.Formula;
                foreach (FamilyParameter p in SourceParameters)
                {
                    if (formula.Contains(p.Definition.Name) == false) continue;
                    string pName = p.Definition.Name;
                    FamilyParameter tprequired = RevitFamilyManager.get_Parameter(pName);
                    if ((tprequired == null) || (type.HasValue(tprequired) == false)
                        || (tprequired.StorageType != p.StorageType)
                        || (tprequired.Definition.ParameterType != p.Definition.ParameterType)
                        || (tpara.IsInstance == false && tprequired.IsInstance))

                    {
                        formula = "";
                        break;
                    }
                    if (SourceParameters.Contains(tprequired))
                    {
                        MessageBox.Show(pName + " should be added next time.");
                        formula = "";
                        break;
                    }
                }
                if (formula.Length > 0)
                {
                    FormulaInfo f = new FormulaInfo();
                    f.formula = parameter.Formula;
                    formulaInfos.Add(parameter, f);
                }
            }
            if (formulaInfos.Count == 0) return false;
            AddSingleTransactionHandler((SingleTransactionHandler)TransferFormulas);
            return true;
        }

        protected void TransferFormulas()
        {
            IList added = new List<FamilyParameter>();
            foreach (KeyValuePair<FamilyParameter, FormulaInfo> pair in formulaInfos)
            {
                FamilyParameter parameter = pair.Key;
                FormulaInfo f = pair.Value;
                TransferFormula(parameter.Definition.Name, f.formula);
            }
        }
        private bool TransferFormula(string name, string formula)
        {
            FamilyParameter targetParameter = RevitFamilyManager.get_Parameter(name);
            RevitFamilyManager.SetFormula(targetParameter, formula);
            return true;
        }
    }
}
