using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Datas;
using DDNRevitAddins.Archive.Environments;

namespace DDNRevitAddins.Archive.Elements
{
    public class GroupInfo
    {
        public Group RevitGroup;
        private GroupType _curGroupType;
        public GroupTypeInfo CurGroupTypeInfo;
        public IList<ElementId> MemberIds;
        public Document DbDocument;
        public int IdValue;
        private string _oldGroupTypeName;
        private GroupTypeInfo _oldGroupTypeInfo;
        private int _oldIdValue;

        public GroupInfo(Group group)
        {
            RevitGroup = group;
            UpdateGroupInfo();
            MemberIds = group.ExtGetMemberIds();
            DbDocument = group.ExtDbDocument();
        }

        public GroupType CurGroupType
        {
            get => _curGroupType;
            set
            {
                RevitGroup.ExtSetType(value);
                _curGroupType = value;
            }
        }

        private void UpdateGroupInfo()
        {
            _curGroupType = RevitGroup.ExtGetType();
            CurGroupTypeInfo = new GroupTypeInfo(CurGroupType);
            IdValue = RevitGroup.ExtIdValue();
        }

        //transaction required.
        public void UnGroup()
        {
            _oldGroupTypeInfo = CurGroupTypeInfo;
            _oldGroupTypeName = _oldGroupTypeInfo.RevitName;
            _oldIdValue = IdValue;
            CurGroupTypeInfo.RevitName = _oldGroupTypeName + TimeExtension.ExtTimeStamp();
            RevitGroup.ExtUnGroup();
        }


        //transaction required.
        public virtual void ReGroup()
        {
            RevitGroup = DbDocument.ExtCreate().ExtNewGroup(MemberIds);
            UpdateGroupInfo();
            CurGroupTypeInfo.RevitName = _oldGroupTypeName;
        }

        //transaction required.
        public void SwapOtherOldGroups()
        {
            foreach (Group group in _oldGroupTypeInfo.GetGroups())
            {
                group.ExtSetType(CurGroupType);
            }
        }

    }
}
