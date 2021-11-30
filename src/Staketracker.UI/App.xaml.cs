using System;
using Staketracker.Core.Validators;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Globalization;
using System.Threading;
using Staketracker.UI.Resources;

namespace Staketracker.UI
{

    public class formDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextBoxTemplate { get; set; }
        public DataTemplate DropDownListTemplate { get; set; }
        public DataTemplate MultiLineTemplate { get; set; }
        public DataTemplate DateTimeTemplate { get; set; }
        public DataTemplate CheckBoxTemplate { get; set; }
        public DataTemplate ListBoxMultiTemplate { get; set; }




        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var s = (KeyValuePair<string, ValidatableObject<string>>)item;
            var k = s.Value.FormAndDropDownField.InputType;
            if (k == "TextBox")
                return TextBoxTemplate;
            else if (k == "MultiLine")
                return MultiLineTemplate;
            else if (k == "DateTime")
                return DateTimeTemplate;
            else if (k == "CheckBox")
                return CheckBoxTemplate;
            else if (k == "ListBoxMulti")
                return ListBoxMultiTemplate;
            else

                return DropDownListTemplate;

        }
    }


    public class formDataTemplateSelectorView : DataTemplateSelector
    {
        public DataTemplate TextBoxTemplate { get; set; }
        public DataTemplate DropDownListTemplate { get; set; }
        public DataTemplate MultiLineTemplate { get; set; }
        public DataTemplate DateTimeTemplate { get; set; }
        public DataTemplate CheckBoxTemplate { get; set; }
        public DataTemplate ListBoxMultiTemplate { get; set; }




        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var s = (KeyValuePair<string, ValidatableObject<string>>)item;
            var k = s.Value.FormAndDropDownField.InputType;
            if (k == "TextBox")
                return TextBoxTemplate;
            else if (k == "MultiLine")
                return MultiLineTemplate;
            else if (k == "DateTime")
                return DateTimeTemplate;
            else if (k == "CheckBox")
                return CheckBoxTemplate;
            else if (k == "ListBoxMulti")
                return ListBoxMultiTemplate;
            else

                return DropDownListTemplate;

        }
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CultureInfo language = new CultureInfo("am");
            Thread.CurrentThread.CurrentUICulture = language;
            //  Resource1.Culture = language;
        }

        private void RadDateTimePicker_SelectionChanged(object sender, EventArgs e)
        {
            // implement your logic here
        }
    }
}
