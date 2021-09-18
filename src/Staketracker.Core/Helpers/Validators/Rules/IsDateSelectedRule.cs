using System;
namespace Staketracker.Core.Validators.Rules
{
    public class IsDateSelectedRule : IDateValidationRule
    {
        public string ValidationMessage { get; set; }


        public bool Check(DateTime? dateTime)
        {

            return dateTime != null;


        }
    }
}
