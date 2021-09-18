using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Staketracker.Core.Validators
{
    public interface IDateValidationRule
    {
        string ValidationMessage { get; set; }
        bool Check(DateTime? dateTime);
    }
}
