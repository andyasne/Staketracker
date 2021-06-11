using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.FieldsValue
{


    public class Field
    {
        public int PrimaryKey { get; set; }
        public string Label { get; set; }
        public string InputType { get; set; }
        public string ValidationType { get; set; }
        public bool MandatoryField { get; set; }
        public string ColumnName { get; set; }
        public List<Staketracker.Core.Models.FormAndDropDownField.DropdownValue> DropdownValues { get; set; }
        public string GroupID { get; set; }
        public string DraftSection { get; set; }
        public object Value { get; set; }
        public int? SelectedKey { get; set; }
        public bool IsHidden { get; set; }
        public bool IsGroupRequired { get; set; }
    }

    public class LinkedTo
    {
        public int Issue { get; set; }
        public int Team { get; set; }
        public int Communications { get; set; }
        public int IndividualStakeholders { get; set; }
        public int GroupedStakeholders { get; set; }
        public int LandParcelStakeholders { get; set; }
    }

    public class D
    {
        public string PrimaryKey { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public IList<Field> Fields { get; set; }
        public LinkedTo LinkedTo { get; set; }
    }

    public class FieldsValue
    {
        public D d { get; set; }
    }


}
