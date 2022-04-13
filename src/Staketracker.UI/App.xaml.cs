using System;
using Staketracker.Core.Validators;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Globalization;
using System.Threading;
using Staketracker.Core.Resources;
using Staketracker.Core.Res;
using Plugin.Settings;

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
        public DataTemplate LinkedToLabelTemplate { get; set; }
        public DataTemplate LinkedToTemplate { get; set; }



        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var s = (KeyValuePair<string, ValidatableObject<string>>)item;
            if (s.Value.FormAndDropDownField != null)
            {
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
                else if (k == "DropDownList")
                    return DropDownListTemplate;
                else
                    return null;
            }
            else if (s.Value.LinkedControlType == "button")
                return LinkedToTemplate;
            else if (s.Value.LinkedControlType == "label")
                return LinkedToLabelTemplate;
            else
                return null;

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
        public DataTemplate LinkedToTemplate { get; set; }

        public DataTemplate LinkedToLabelTemplate { get; set; }



        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var s = (KeyValuePair<string, ValidatableObject<string>>)item;
            if (s.Value.FormAndDropDownField != null)
            {
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
                else if (k == "DropDownList")
                    return DropDownListTemplate;
                else
                    return null;
            }
            else if (s.Value.LinkedControlType == "button")
                return LinkedToTemplate;
            else if (s.Value.LinkedControlType == "label")
                return LinkedToLabelTemplate;
            else
                return null;

        }
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string defaultLang = CrossSettings.Current.GetValueOrDefault("DefaultLanguage", "en");
            CultureInfo language = new CultureInfo(defaultLang);
            Thread.CurrentThread.CurrentUICulture = language;
            AppRes.Culture = language;
        }

        private void RadDateTimePicker_SelectionChanged(object sender, EventArgs e)
        {
            // implement your logic here
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}
