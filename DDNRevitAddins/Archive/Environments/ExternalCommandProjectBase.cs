using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Geometries;
using DDNRevitAddins.Archive.Elements;

namespace DDNRevitAddins.Archive.Environments
{
    public abstract class ExternalCommandProjectBase : ExternalCommandBase
    {
        public Autodesk.Revit.Creation.Document RevitCreateDocument;
        private const int _symbolSize=1;

        internal override bool InitiateRevitAccessor(ExternalCommandData commandData)
        {
            if (!base.InitiateRevitAccessor(commandData)) return false;
            ThrowExceptionIfNoProjectDocument();
            RevitCreateDocument = RevitDbDocument.ExtCreate();
            return true;
        }

        internal void ShowText(string content, XYZ ptLocation)
        {
            var defaultTextTypeId = RevitDbDocument.ExtGetDefaultTextNoteType();
            TextNoteExtension.ExtCreate(RevitDbDocument, RevitActiveView.Id, ptLocation, content,
                defaultTextTypeId);
        }
        internal void ShowPtViaCircle(XYZ pt)
        {
            Arc arc=ArcExtension.ExtCreateCircle(pt, _symbolSize);
            ShowCurve(arc);
        }
        internal void ShowPtViaCircle(XYZ pt,string name)
        {
            ShowPtViaCircle(pt);
            ShowText(name,pt);
        }
        internal void ShowCurve(Curve curve)
        {
            RevitCreateDocument.ExtNewModelCurve(curve, RevitActiveView.ExtSketchPlane());
        }

        internal void ShowCurveWithNameAtMid(Curve curve, string name)
        {
            ShowCurve(curve);
            ShowText(name,curve.ExtGetMidPoint());
        }
        internal void ShowVector(XYZ vector, XYZ ptOrigin)
        {
            ShowPtViaCircle(ptOrigin);
            ShowCurve(Line.CreateBound(ptOrigin, ptOrigin.ExtAdd(vector)));
        }
        internal Line ShowLine(XYZ pt1, XYZ pt2)
        {
            Line line = Line.CreateBound(pt1, pt2);
            ShowCurve(line) ;
            return line;
        }

        internal void ShowLine(XYZ pt1, XYZ pt2, string name)
        {
            Line line = ShowLine(pt1, pt2);
            ShowText(name, line.ExtGetMidPoint());
        }
    }
}