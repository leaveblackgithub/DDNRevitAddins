using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MultipleCutFix : ExternalCommandProjectBase
    {

        public string StatusPromptToPickCuttingElement = "Pick the element to uncut with";
        private Element _cuttingElement;
        private IList<Element> _elementsToBeUnCut;

        protected override bool RunBeforeTransaction()
        {
            if (!GetCuttingElement()) return false;
            if (!GetElementsToBeUnCut()) return false;
            AddSingleTransactionHandler((SingleTransactionHandler)UnCutElements);
            return true;
        }



        private void UnCutElements()
        {
            ElementIntersectsElementFilter filter=new ElementIntersectsElementFilter(_cuttingElement);
            foreach (Element e in _elementsToBeUnCut)
            {
//                if (!filter.ExtPassesFilter(e))
//                {
//                    RevitDbDocument.ExtRemoveCutBetweenSolids(e, _cuttingElement);
//                    continue;
//                }
                e.ExtSplitFacesOfCuttingSolid(_cuttingElement,false);
            }
        }

        private bool GetCuttingElement()
        {
            _cuttingElement = PickElement(StatusPromptToPickCuttingElement);
            if (_cuttingElement == null) return false;
            DebugExtension.PrintFieldValue(nameof(_cuttingElement), nameof(_cuttingElement), _cuttingElement.ExtIdValue());
            return true;
        }



        private bool GetElementsToBeUnCut()
        {
            _elementsToBeUnCut = RevitDbDocument.ExtGetElementsBeingCut(_cuttingElement);
            if (_elementsToBeUnCut == null || !_elementsToBeUnCut.Any()) return false;
            return true;
        }
    }
}