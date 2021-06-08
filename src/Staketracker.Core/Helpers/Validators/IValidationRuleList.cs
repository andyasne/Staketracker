using System;
using System.Collections.Generic;

namespace Staketracker.Core.Validators
{
    public interface IValidationRuleList
    {
        string ValidationMessage { get; set; }
        bool Check(List<Models.FormAndDropDownField.DropdownValue> selectedValues);
    }
}
