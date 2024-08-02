using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Elements
{
    public class GroupTypeInfo
    {
        public GroupType RevitGroupType;
        private string _revitName;

        public GroupTypeInfo(GroupType groupType)
        {
            RevitGroupType = groupType;
            _revitName = groupType.ExtGetName();
        }

        internal virtual GroupSet GetGroups()
        {
            return RevitGroupType.ExtGroups();
        }

        public string RevitName
        {
            get => _revitName;
            set
            {
                RevitGroupType.ExtSetName(value);
                _revitName = value;
            }
        }
    }
}
