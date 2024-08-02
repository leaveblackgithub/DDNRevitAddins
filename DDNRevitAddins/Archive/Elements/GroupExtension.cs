using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Elements
{
    public static class GroupExtension
    {

        public static IList<ElementId> ExtGetMemberIds(this Group group)
        {
            return group.GetMemberIds();
        }
        public static string ExtGetName(this Group group)
        {
            return group.Name;
        }
        public static GroupType ExtGetType(this Group group)
        {
            return group.GroupType;
        }
        public static void ExtSetType(this Group group, GroupType newType)
        {
            group.GroupType=newType;
        }

        public static void ExtUnGroup(this Group group)
        {
            group.UngroupMembers();
        }

        public static ISet<ElementId> ExtGetAvailableAttachedDetailGroupTypeIds(this Group group)
        {
            return group.GetAvailableAttachedDetailGroupTypeIds();
        }

        public static ElementId ExtAttachedParentId(this Group group)
        {
            return group.AttachedParentId;
        }

        public static void ExtShowAttachedDetailGroups(this Group group, View view, ElementId detailGroupTypeId)
        {
            group.ShowAttachedDetailGroups(view, detailGroupTypeId);
        }

}
}
