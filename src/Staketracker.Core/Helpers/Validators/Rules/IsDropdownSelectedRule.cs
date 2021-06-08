using System.Collections.Generic;
using Staketracker.Core.Models.FormAndDropDownField;

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
    }
}
