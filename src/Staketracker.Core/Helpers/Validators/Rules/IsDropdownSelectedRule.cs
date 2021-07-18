using Staketracker.Core.Models.FormAndDropDownField;
using System.Collections.Generic;

namespace Staketracker.Core.Validators.Rules
{
    public class IsDropdownSelectedRule : IValidationRuleList
    {
        public string ValidationMessage { get; set; }


        public bool Check(List<DropdownValue> selectedValues)
        {
            if (selectedValues != null && selectedValues.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Check(DropdownValue selectedValue)
        {
            if (selectedValue != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
