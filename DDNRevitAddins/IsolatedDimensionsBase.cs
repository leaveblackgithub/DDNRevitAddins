using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using DDNRevitAddins.Archive.Environments;
using DDNRevitAddins.Archive.Geometries;

namespace DDNRevitAddins
{
    public class IsolatedDimensionsBase : ExternalCommandProjectBase
    {
        internal GraphicsStyle _dimensionLineStyle;
        internal const string LineStyleName = "A-ANNO-DIMS";
        internal XYZ _normal;
        internal XYZ _origin;
        internal XYZ _vectorOfDimLine;
        internal List<ElementId> _idset;
        internal double _length;
        internal ObjectSnapTypes _osnap;
        internal IList<Reference> _referenceArrayOfDimLines;
        internal List<XYZ> _dimensionPoints;

        protected override bool RunBeforeTransaction()
        {
            _length = RevitActiveView.ExtGetScale() / 304.8 / 5;
            _origin = RevitActiveView.Origin;
            _normal = RevitActiveView.ViewDirection;
            _referenceArrayOfDimLines = new List<Reference>();
            _idset = new List<ElementId>();
            _dimensionPoints = new List<XYZ>();
            _osnap = ObjectSnapTypes.Centers | ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections |
                     ObjectSnapTypes.Midpoints | ObjectSnapTypes.Nearest | ObjectSnapTypes.Perpendicular |
                     ObjectSnapTypes.Points | ObjectSnapTypes.Quadrants | ObjectSnapTypes.Tangents;
            AddSingleTransactionHandler((SingleTransactionHandler)SetWorkPlane);
            AddSingleTransactionHandler((SingleTransactionHandler)GetProjectLineStyleForDims);
            return true;
        }

        internal void GetProjectLineStyleForDims()
        {
            _dimensionLineStyle = GetProjectLineStyle(LineStyleName);
        }

        public void SetWorkPlane()
        {
            if (RevitActiveView.SketchPlane != null) return;
            RevitActiveView.SketchPlane = RevitDbDocument.ExtCreateSketchPlaneByActiveView();
        }

        internal XYZ ProjectPoint(XYZ pt)
        {
            var v = pt.ExtSubtract(_origin);
            var k = v.X * _normal.X + v.Y * _normal.Y + v.Z * _normal.Z;
            var ptP = _origin.ExtAdd(_normal.ExtMultiply(k));
            return _origin.ExtAdd(pt.ExtSubtract(ptP));
        }

        internal void CreateDimAndGroup()
        {
            var dim = CreateDims();
            _idset.Add(dim.Id);
            var group = RevitCreateDocument.NewGroup(_idset);
            @group.GroupType.Name = "DDNDIM_" + @group.GroupType.Name;
        }

        internal virtual Dimension CreateDims()
        {
            throw new NotImplementedException();
        }

        internal DetailCurve CreateDetailLinesForDims(XYZ pt)
        {
            var line = LineExtension.ExtCreateBound(pt, pt.ExtAdd(_vectorOfDimLine));
            var dline = RevitCreateDocument.NewDetailCurve(RevitActiveView, line);
            dline.LineStyle = _dimensionLineStyle;
            _idset.Add(dline.Id);
            _referenceArrayOfDimLines.Add(dline.GeometryCurve.Reference);
            return dline;
        }
    }
}