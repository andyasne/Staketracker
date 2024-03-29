using System;
using System.Collections.Generic;

namespace Staketracker.Core.Models.FormAndDropDownField
{
    public class DropdownValue
    {
        public string PrimaryKey { get; set; }
        public string Label { get; set; }
    }

    public class D
    {
        public int PrimaryKey { get; set; }
        public string Label { get; set; }
        public string InputType { get; set; }
        public string ValidationType { get; set; }
        public bool MandatoryField { get; set; }
        public string ColumnName { get; set; }
        public List<DropdownValue> DropdownValues { get; set; }
        public string GroupID { get; set; }
        public string DraftSection { get; set; }
        public bool IsHidden { get; set; }
        public bool IsGroupRequired { get; set; }
    }

    public class FormAndDropDownField
    {
        public IList<D> d { get; set; }
    }


}
