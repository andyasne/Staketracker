using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Staketracker.Core.Validators
{
    public class DropdownValues
    {

        public DropdownValues()
        {

        }
        public string PrimaryKey { get; set; }
        public string Label { get; set; }
    }

    public class ValidatableObject<T> : IValidatable<T>
    {
        public ValidatableObject()
        {
            IsValid = true;
            Errors = new List<string>();

        }

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
        public List<IValidationRuleList> ValidationsList { get; } = new List<IValidationRuleList>();

        public List<Models.FormAndDropDownField.DropdownValue> DropdownValues { get; set; } = new List<Models.FormAndDropDownField.DropdownValue>();
        public List<Models.FormAndDropDownField.DropdownValue> SelectedItems { get; set; } = new List<Models.FormAndDropDownField.DropdownValue>();
        public Models.FormAndDropDownField.DropdownValue SelectedItem { get; set; } = new Models.FormAndDropDownField.DropdownValue();
        public string Label { get; set; }

        public Models.FormAndDropDownField.D FormAndDropDownField { get; set; }

        public List<string> _Errors;
        public List<string> Errors { get => _Errors; set => SetField(ref _Errors, value); }

        public bool CleanOnChange { get; set; } = true;

        T _value;
        public T Value
        {
            get => _value;
            set
            {

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
            IEnumerable<string> errors;

            if (DropdownValues != null && DropdownValues.Count > 0)
            {
                errors = ValidationsList.Where(v => !v.Check(SelectedItems))
                .Select(v => v.ValidationMessage);
            }
            else
            {
                errors = Validations.Where(v => !v.Check(Value))
              .Select(v => v.ValidationMessage);

            }

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
