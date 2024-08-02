using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Elements
{
    public class DetailGroupInfo:GroupInfo
    {
        public ElementId CurViewId;
        public DetailGroupInfo(Group group) : base(group)
        {
            CurViewId = group.ExtOwnerViewId();
        }
    }
}
