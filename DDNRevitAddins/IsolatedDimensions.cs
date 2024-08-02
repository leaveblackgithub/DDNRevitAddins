using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Datas;
using DDNRevitAddins.Archive.Environments;
using DDNRevitAddins.Archive.Geometries;
using DDNRevitAddins.General;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class IsolatedDimensions : IsolatedDimensionsBase
    {

        internal override Dimension CreateDims()
        {
            return RevitCreateDocument.ExtNewDimensionFrRefList(RevitActiveView,
                LineExtension.ExtCreateUnBound(_dimensionPoints[0], _dimensionPoints[1].Subtract(_dimensionPoints[0])), _referenceArrayOfDimLines);
        }


        protected override bool RunBeforeTransaction()
        {
            base.RunBeforeTransaction();

            AddSingleTransactionHandler((SingleTransactionHandler)CreateFirstTwoDimLines);
            AddContinuousTransactionHandler((ContinuousTransactionHandler)ContinuousCreateSecondLine,(ContinuousTransactionAfterHandler)CreateDimAndGroup);
           
            return true;
        }

        private void ContinuousCreateSecondLine()
        {
            var pt2 = RevitSelection.ExtPickPoint(_osnap, "Select second point");
            if(pt2==null)throw new ExceptionToCancelContinuousTransaction("");
            _dimensionPoints.Add(ProjectPoint(pt2));

            CreateDetailLinesForDims(pt2);
        }

        private void CreateFirstTwoDimLines()
        {
            var pt1 = RevitSelection.ExtPickPoint(_osnap, "Select first point");
            if (pt1 == null) throw new ExceptionToCancel("");
            _dimensionPoints.Add(ProjectPoint(pt1));
            var pt2 = RevitSelection.ExtPickPoint(_osnap, "Select second point To define dim line");
            if (pt2 == null) throw new ExceptionToCancel("");
            _dimensionPoints.Add(ProjectPoint(pt2));
            _vectorOfDimLine = pt1.ExtSubtract(pt2).ExtCrossProduct(_normal);
            _vectorOfDimLine = _vectorOfDimLine.ExtDivide(_vectorOfDimLine.GetLength()).ExtMultiply(_length);

            CreateDetailLinesForDims(pt1);
            CreateDetailLinesForDims(pt2);
        }
    }
}