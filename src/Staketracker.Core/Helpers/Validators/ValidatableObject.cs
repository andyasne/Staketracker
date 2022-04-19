using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Staketracker.Core.Models.FormAndDropDownField;
using Xamarin.Forms;

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

            this.SelectAllCommand = new Command(this.OnSelectAllCommandExecute);
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


        public ICommand SelectAllCommand { get; set; }


        private ObservableCollection<object> selectedItems = new ObservableCollection<object>();

        private bool? selectAllChecked = false;
        private bool isInternalCheckChanged;

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return this.selectedItems;
            }
            set
            {
                if (this.selectedItems != value)
                {
                    if (this.selectedItems != null)
                    {
                        //   this.selectedItems.CollectionChanged -= this.OnSelectedItemsCollectionChanged;
                    }

                    this.selectedItems = value;

                    if (this.selectedItems != null)
                    {
                        //    this.selectedItems.CollectionChanged += this.OnSelectedItemsCollectionChanged;
                    }

                    OnPropertyChanged("selectedItems");
                }
            }
        }

        public bool? SelectAllChecked
        {
            get
            {
                return this.selectAllChecked;
            }
            set
            {
                if (this.selectAllChecked != value)
                {
                    this.selectAllChecked = value;

                    if (!this.isInternalCheckChanged && this.selectAllChecked.HasValue)
                    {
                        if (this.selectAllChecked.Value)
                        {
                            foreach (var store in this.DropdownValues)
                            {
                                if (!this.SelectedItems.Contains(store))
                                {
                                    this.SelectedItems.Add(store);
                                }
                            }
                        }
                        else
                        {
                            this.SelectedItems.Clear();
                        }
                    }

                    this.OnPropertyChanged("selectAllChecked");
                }
            }
        }

        private void OnSelectAllCommandExecute(object obj)
        {
            if (this.selectAllChecked == null)
            {
                this.SelectAllChecked = false;
            }
            else
            {
                this.SelectAllChecked = !this.selectAllChecked;
            }
        }

        private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var action = e.Action;
            if (action == NotifyCollectionChangedAction.Add)
            {
                this.isInternalCheckChanged = true;
                if (this.SelectedItems.Count == this.DropdownValues.Count)
                {
                    this.SelectAllChecked = true;
                }
                else
                {
                    this.SelectAllChecked = null;
                }
                this.isInternalCheckChanged = false;

                return;
            }

            if (action == NotifyCollectionChangedAction.Remove)
            {
                this.isInternalCheckChanged = true;
                if (this.SelectedItems.Count == 0)
                {
                    this.SelectAllChecked = false;
                }
                else
                {
                    this.SelectAllChecked = null;
                }
                this.isInternalCheckChanged = false;
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

        private bool _IsChecked;
        public bool IsChecked
        {
            get => _IsChecked;
            set => SetField(ref _IsChecked, value);
        }

        public string PageTitle { get; set; }

        public virtual bool Validate()
        {
            Errors.Clear();
            IEnumerable<string> errors;

            if (isSelectMultiple)
            {
                errors = null;
                //= ValidationsList.Where(v => !v.Check(SelectedItems))
                //    .Select(v => v.ValidationMessage);
            }

            else if (isSelectOne)
            {
                errors = ValidationsList.Where(v => !v.Check(SelectedItem))
                    .Select(v => v.ValidationMessage);

            }
            else if (isDateType)
            {

                //errors = ValidationsDateTime.Where(v => !v.Check(SelectedDate))
                //    .Select(v => v.ValidationMessage);

            }
            else
            {
                errors = Validations.Where(v => !v.Check(Value))
              .Select(v => v.ValidationMessage);

            }
            try
            {
                Errors = errors.ToList();
                IsValid = !Errors.Any();
            }
            catch (Exception ex)
            {

            }

            return IsValid;
        }
        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
