using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using DDNRevitAddins.Archive.Datas;
using DDNRevitAddins.Archive.Elements;
using DDNRevitAddins.Archive.Environments;

namespace DDNRevitAddins
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class FamilyParameterAssociate : ExternalCommandFamilyBase
    {
        private IList<Element> _selectedFamilyInstances;

        private Dictionary<string, FamilyParameter> _familyParameters;
        /*Document _doc;
        Dictionary<string, FamilyParameter> _fParas;
        Dictionary<string, Parameter> _paras;

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIApplication app = commandData.Application;
                UIDocument uidoc = app.ActiveUIDocument;                
                _doc = uidoc.Document;

                IList<Reference> refObjs = uidoc.Selection.PickObjects(ObjectType.Element,new FamilyInstanceSelectionFilter(_doc));
                Transaction transaction = new Transaction(_doc);
                _fParas = _doc.FamilyManager.Parameters
                    .Cast<FamilyParameter>()
                    .ToDictionary(p => p.Definition.Name);
                transaction.Start("Associate Parameters");

                foreach (Reference refObj in refObjs)
                {
                    FamilyInstance instance = _doc.GetElement(refObj) as FamilyInstance;

                    _paras = instance.Parameters
                        .Cast<Parameter>()
                        .ToDictionary(p => p.Definition.Name);

                    foreach (KeyValuePair<string,Parameter> pair in _paras)
                    {
                        try
                        {
                            AssociateParameter(pair.Key, instance);
                        }
                        catch
                        {
                        }
                    }
                }
                transaction.Commit();
                return Result.Succeeded;
            }

            //命令异常退出
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return Result.Failed;
            }
        }*/

        private bool AssociateParameter(Parameter parameter, 
            FamilyInstance instance)
        {
            try
            {
                FamilyParameter fparameter;
                string parameterName = parameter.ExtGetDefinitionName();
                if (RevitFamilyManager.ExtCanElementParameterBeAssociated(parameter) == false) return false;
                if (_familyParameters.TryGetValue(parameterName, out fparameter) == false) return false;    

                if(parameter.ExtGetParameterType()!= fparameter.ExtGetParameterType()) return false;

                RevitFamilyManager.ExtAssociateElementParameterToFamilyParameter(parameter, fparameter);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected override bool RunBeforeTransaction()
        {
            _selectedFamilyInstances = PickElementsOfFamilyInstance("Select Components to associate parameters");
            if (_selectedFamilyInstances.Count == 0) return false;
            _familyParameters = RevitFamilyManager.ExtGetFamilyParameterDictionary();
            AddSingleTransactionHandler((SingleTransactionHandler)AssociateParameterToInstances);
            return true;
        }

        protected void AssociateParameterToInstances()
        {
            foreach (Element e in _selectedFamilyInstances)
            {
                FamilyInstance instance = (FamilyInstance) e;
                IList<Parameter> _paras = instance.ExtGetParameterList();

                foreach (Parameter  parameter in _paras)
                {
                    try
                    {
                        AssociateParameter(parameter, instance);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

}