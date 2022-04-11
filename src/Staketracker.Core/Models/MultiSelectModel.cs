using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Telerik.XamarinForms.Common;

namespace Staketracker.Core.Models
{
    public class MultiSelectModel : NotifyPropertyChangedBase
    {
        public MultiSelectModel(int id, string title, bool isSelected, object attach)
        {
            Id = id;
            Title = title;
            IsSelected = isSelected;
            this.attach = attach;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        private bool? isSelected;

        public bool? IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        public object attach { get; set; }


    }
}
