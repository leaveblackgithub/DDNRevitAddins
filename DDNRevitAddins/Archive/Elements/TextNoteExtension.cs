using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Elements
{
    public static class TextNoteExtension
    {
        public static void ExtCreate(
            Document document,
            ElementId viewId,
            XYZ position,
            string text,
            ElementId typeId

        )
        {
            TextNote.Create(document, viewId, position, text, typeId);
        }

	}
}
