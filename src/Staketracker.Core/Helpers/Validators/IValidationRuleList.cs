using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Staketracker.Core.Validators
{
    public interface IValidationRuleList
    {
        string ValidationMessage { get; set; }
        bool Check(ObservableCollection<Models.FormAndDropDownField.DropdownValue> selectedValues);
        bool Check(Models.FormAndDropDownField.DropdownValue selectedValue);
    }
}
