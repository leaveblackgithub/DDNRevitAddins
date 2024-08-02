using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using DDNRevitAddins.Archive.Datas;
using DDNRevitAddins.Archive.Environments;

namespace DDNRevitAddins.Archive.Elements
{
    public class ModelGroupInfo:GroupInfo
    {
        public IList<DetailGroupInfo> DetailGroupInfos;
        public ModelGroupInfo(Group group) : base(group)
        {
            GetAttachedDetailGroups();
        }

        private void GetAttachedDetailGroups()
        {
            DetailGroupInfos = new List<DetailGroupInfo>();
            ISet<ElementId> availableAttachedDetailGroupTypeIds = RevitGroup.ExtGetAvailableAttachedDetailGroupTypeIds();
            foreach (ElementId id in availableAttachedDetailGroupTypeIds)
            {
                if (id.ExtIntegerValue() < 0) continue;
                GroupType groupType = DbDocument.GetElement(id) as GroupType;
                GroupTypeInfo groupTypeInfo = new GroupTypeInfo(groupType);
                foreach (Group group in groupTypeInfo.GetGroups())
                {
                    if (group.ExtAttachedParentId().IntegerValue != IdValue) continue;
                    DetailGroupInfo detailGroupInfo = new DetailGroupInfo(group);
                    //TaskDialogExtension.ExtShow("test", detailGroupInfo.CurViewId.ToString());
                    DetailGroupInfos.Add(detailGroupInfo);
                }
            }
        }

        public void ShowAttachedDetailGroups()
        {
            foreach (DetailGroupInfo detailGroupInfo in DetailGroupInfos)
            {
                View view = DbDocument.GetElement(detailGroupInfo.CurViewId) as View;
                RevitGroup.ExtShowAttachedDetailGroups(view,new ElementId( detailGroupInfo.IdValue));
            }
        }

    }
}
