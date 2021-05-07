using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Staketracker.Core.Validators
{
    public class ValidatableObject<T> : IValidatable<T>
    {
        public ValidatableObject()
        {
            IsValid = true;
          Errors = new List<string>();

    }

    // boiler-plate
    public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
       
         
        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        public List<string> _Errors;
        public List<string> Errors { get=>_Errors; set => SetField(ref _Errors, value); } 

        public bool CleanOnChange { get; set; } = true;

        T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;

                if (CleanOnChange)
                    IsValid = true;
                SetField(ref _value, value);
            }
        }

        private bool _IsValid;
        public bool IsValid
        {
            get => _IsValid;
            set => SetField(ref _IsValid, value);
        } 

        public virtual bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }
        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
