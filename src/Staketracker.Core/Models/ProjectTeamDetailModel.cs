using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.ProjectTeamDetail
{
    public class DropdownValue
    {
        public string PrimaryKey { get; set; }
        public string Label { get; set; }
    }

    public class Field
    {
        public int PrimaryKey { get; set; }
        public string Label { get; set; }
        public string InputType { get; set; }
        public string ValidationType { get; set; }
        public bool MandatoryField { get; set; }
        public string ColumnName { get; set; }
        public IList<DropdownValue> DropdownValues { get; set; }
        public string GroupID { get; set; }
        public object DraftSection { get; set; }
        public string Value { get; set; }
        public int? SelectedKey { get; set; }
        public bool IsHidden { get; set; }
        public bool IsGroupRequired { get; set; }
    }

    public class LinkedTo
    {
        public int Event { get; set; }
    }

    public class D
    {
        public string PrimaryKey { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public IList<Field> Fields { get; set; }
        public LinkedTo LinkedTo { get; set; }
    }

    public class ProjectTeamDetailModel
    {
        public D d { get; set; }
    }


}
