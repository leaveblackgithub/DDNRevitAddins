using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Elements;
using DDNRevitAddins.Archive.Environments;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class MultipleCut : ExternalCommandProjectBase
    {

        private const string StatusPromptToElementsToBeCut = "Pick the elements to be cut";
        public string StatusPromptToPickCuttingElement = "Pick the element to cut with";
        private Element _cuttingElement;
        private IList<Element> _elementsToBeCut;

        protected override bool RunBeforeTransaction()
        {
            if (!GetElementsToBeCut()) return false;
            if (!GetCuttingElement()) return false;
            AddSingleTransactionHandler((SingleTransactionHandler) CutElements);
            return true;
        }


//        protected override bool RunInTransaction()
//        {
//            CutElements();
//            return true;
//        }


        private void CutElements()
        {
            foreach (Element e in _elementsToBeCut)
            {
                RevitDbDocument.ExtAddCutBetweenSolids(e,_cuttingElement);
            }
        }


        private bool GetCuttingElement()
        {
            _cuttingElement = PickElement(StatusPromptToPickCuttingElement);
            if (_cuttingElement == null) return false;
            DebugExtension.PrintFieldValue(nameof(_cuttingElement), nameof(_cuttingElement), _cuttingElement.ExtIdValue());
            return true;
        }



        private bool GetElementsToBeCut()
        {
            _elementsToBeCut = PickElements(StatusPromptToElementsToBeCut);
            if (_elementsToBeCut == null) return false;
            DebugExtension.PrintFieldValue(nameof(GetElementsToBeCut), nameof(_elementsToBeCut), _elementsToBeCut.Count);
            return true;
        }
    }
}
