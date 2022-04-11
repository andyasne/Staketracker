using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models
{
    public class MultiSelectModel
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
        public bool IsSelected { get; set; }
        public object attach { get; set; }


    }
}
