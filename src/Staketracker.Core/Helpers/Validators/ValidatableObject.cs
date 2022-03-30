using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Staketracker.Core.Models.FormAndDropDownField;

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

    public class ValidatableObject<T> : IValidatable<T>, INotifyPropertyChanged
    {
        public ValidatableObject()
        {
            IsValid = true;
            Errors = new List<string>();
            DefaultHighlightedDate = DateTime.Today;
            SelectedDate = null;
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
        public List<IDateValidationRule> ValidationsDateTime { get; set; } = new List<IDateValidationRule>();
        public List<IValidationRuleList> ValidationsList { get; } = new List<IValidationRuleList>();
        public ObservableCollection<Models.FormAndDropDownField.DropdownValue> DropdownValues { get; set; } = new ObservableCollection<Models.FormAndDropDownField.DropdownValue>();
        private ObservableCollection<Models.FormAndDropDownField.DropdownValue> selectedItems { get; set; }
        public ObservableCollection<Models.FormAndDropDownField.DropdownValue> SelectedItems
        {
            get
            {
                return this.selectedItems;
            }
            set
            {

                this.selectedItems = value;

                OnPropertyChanged("SelectedItems");



            }
        }




        public DateTime? defaultHighlightedDate;
        public DateTime? DefaultHighlightedDate
        {
            get
            {

                return defaultHighlightedDate;
            }
            set
            {
                //if (this.selectedDate != value)
                {
                    this.defaultHighlightedDate = value;

                    SetField(ref defaultHighlightedDate, value);



                }
            }
        }




        public DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get
            {

                return selectedDate;
            }
            set
            {
                //if (this.selectedDate != value)
                {
                    this.selectedDate = value;

                    SetField(ref selectedDate, value);

                    OnPropertyChanged("selectedDate");
                    OnPropertyChanged("SelectedDateToString");


                }
            }
        }

        public String SelectedDateToString
        {
            set { }
            get
            {
                if (selectedDate != null)
                {
                    return selectedDate.Value.ToString("MMMM dd yyyy");
                }
                else
                    return "";
            }
        }


        private int selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }
            set
            {
                if (this.selectedIndex != value)
                {
                    this.selectedIndex = value;

                }
            }
        }


        private Models.FormAndDropDownField.DropdownValue selectedItem;//= new Models.FormAndDropDownField.DropdownValue();



        public Boolean isSelectOne { get; set; }
        public string LinkedControlType { get; set; }
        public Boolean isSelectMultiple { get; set; }
        public object Attachment { get; set; }
        public Boolean isDateType { get; set; }
        public Models.FormAndDropDownField.DropdownValue SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                if (this.selectedItem != value)
                {
                    this.selectedItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));


                }
            }
        }


        public string PrimaryKey { get; set; }

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
        public string PageTitle { get; set; }

        public virtual bool Validate()
        {
            Errors.Clear();
            IEnumerable<string> errors;

            if (isSelectMultiple)
            {
                errors = ValidationsList.Where(v => !v.Check(SelectedItems))
                    .Select(v => v.ValidationMessage);
            }

            else if (isSelectOne)
            {
                errors = ValidationsList.Where(v => !v.Check(SelectedItem))
                    .Select(v => v.ValidationMessage);

            }
            else if (isDateType)
            {

                errors = ValidationsDateTime.Where(v => !v.Check(SelectedDate))
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
