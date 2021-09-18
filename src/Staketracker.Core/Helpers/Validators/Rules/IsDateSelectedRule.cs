using System;
namespace Staketracker.Core.Validators.Rules
{
    public class IsDateSelectedRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value is DateTime bday)
            {
                return bday != null;
            }

            return false;
        }
    }
}
