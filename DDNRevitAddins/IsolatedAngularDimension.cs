using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DDNRevitAddins.Archive.Elements;
using DDNRevitAddins.Archive.Environments;
using DDNRevitAddins.Archive.Geometries;
using DDNRevitAddins.General;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class IsolatedAngularDimension : IsolatedDimensionsBase
    {
        internal List<Line> _dimensionEdges;
        private Arc _dimCircle;
        private XYZ _ptCen;
        private XYZ _ptDim;

        protected override bool RunBeforeTransaction()
        {
            base.RunBeforeTransaction();
            _dimensionEdges = new List<Line>();
            AddSingleTransactionHandler((SingleTransactionHandler)CreateEdgeDimLines);
            AddSingleTransactionHandler((SingleTransactionHandler)CreateDimAndGroup);
            return true;
        }
        private void CreateEdgeDimLines()
        {
            GetPointsOnEdge(1);
            GetPointsOnEdge(2);
            _ptDim = RevitSelection.ExtPickPoint(_osnap, "Select point to locate dim");
            if (_ptDim == null) throw new ExceptionToCancel("");
            _ptCen = _dimensionEdges[0].ExtIntersectLine(_dimensionEdges[1]);
            //ShowPtViaCircle(_ptDim, nameof(_ptDim));
            //ShowPtViaCircle(_ptCen,nameof(_ptCen));
            //ShowCurve(LineExtension.ExtCreateBound(_ptCen,_ptDim));
            if (_ptCen==null) throw new ExceptionToCancel("Edges must have one intersection point");
            double radius = _ptDim.ExtSubtract(_ptCen).ExtGetLength();
            _dimCircle = ArcExtension.ExtCreateCircle(_ptCen, radius);
            //ShowCurve(_dimCircle);
            CreateEdgeDimLine(0);
            CreateEdgeDimLine(1);

        }
        internal override Dimension CreateDims()
        {
            return AngularDimensionExtension.ExtCreateDefaultType(RevitDbDocument, RevitActiveView,
                ArcExtension.ExtCreate3Pt(_dimensionPoints[0], _dimensionPoints[1], _ptDim), _referenceArrayOfDimLines
            );
        }

        private void CreateEdgeDimLine(int edgeCount)
        {
            XYZ ptEdge = _dimensionEdges[edgeCount].ExtIntersectCurveCloseTo(_dimCircle, _dimensionPoints[edgeCount]);
            
            if (ptEdge == null) throw new ExceptionToCancel("Edge doesn't intersect dim circle");
            _dimensionPoints[edgeCount] = ptEdge;
            _vectorOfDimLine = ptEdge.ExtSubtract(_ptCen).ExtNormalize().ExtMultiply(_length);
            //ShowVector(_vectorOfDimLine,ptEdge);
            CreateDetailLinesForDims(ptEdge);


        }
        private void GetPointsOnEdge(int edgeCount)
        {
            var pt1 = RevitSelection.ExtPickPoint(_osnap, "Select first point on Edge " + edgeCount );
            if (pt1 == null) throw new ExceptionToCancel("");
            _dimensionPoints.Add(pt1);
            var pt2 = RevitSelection.ExtPickPoint(_osnap, "Select Second point on Edge " + edgeCount);
            if (pt2 == null) throw new ExceptionToCancel("");
            Line line = LineExtension.ExtCreateBoundWithExtension(pt1,pt2);
            _dimensionEdges.Add(line);
            //ShowCurveWithNameAtMid(line,"line" + edgeCount.ToString());

        }
    }
}