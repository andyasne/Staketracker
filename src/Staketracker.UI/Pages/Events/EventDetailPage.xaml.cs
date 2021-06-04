using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Events;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.Events
{


    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class EventDetailPage : MvxContentPage<SEventDetailViewModel>
    {
        public EventDetailPage()
        {
            InitializeComponent();

            //  if (Device.Idiom == TargetIdiom.Phone)
            //{
            this.detailView.PropertyChanged += this.HandleVendorDetailViewPropertyChanged;
            this.editView = new EventsEditPage();
            this.editView.PropertyChanged += this.HandleVendorEditViewPropertyChanged;

            optionsToolbarItem = new ToolbarItem();
            optionsToolbarItem.Text = "Layout";
            optionsToolbarItem.IconImageSource = new FileImageSource() { File = "ellipsis" };
            optionsToolbarItem.Clicked += this.OptionsToolbarItem_Clicked;

            checkToolbarItem = new ToolbarItem();
            checkToolbarItem.Text = "Save";
            checkToolbarItem.IconImageSource = new FileImageSource() { File = "check" };
            checkToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("CommitCommand"));
            // }
            //else
            //{
            //    this.editView = new VendorEditViewTablet();

            //    NavigationPage.SetHasNavigationBar(this, false);
            //}

            this.editView.IsVisible = false;
            var trigger = new DataTrigger(editView.GetType());
            trigger.Binding = new Binding("IsEditing");
            trigger.Value = bool.TrueString;
            trigger.Setters.Add(new Setter() { Property = ContentView.IsVisibleProperty, Value = true });
            this.editView.Triggers.Add(trigger);
            this.LayoutRoot.Children.Add(editView);
        }

        private ContentView editView;
        private ToolbarItem optionsToolbarItem, checkToolbarItem;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.Idiom != TargetIdiom.Phone)
                return;

            if (this.detailView.IsVisible)
            {
                if (!this.ToolbarItems.Contains(optionsToolbarItem))
                    this.ToolbarItems.Add(optionsToolbarItem);
            }

            if (this.editView.IsVisible)
            {
                if (!this.ToolbarItems.Contains(checkToolbarItem))
                    this.ToolbarItems.Add(checkToolbarItem);
            }
        }

        private void OptionsToolbarItem_Clicked(object sender, EventArgs e)
        {
            this.detailView.OpenPopup();
        }

        private void HandleVendorDetailViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IsVisibleProperty.PropertyName)
            {
                if (this.detailView.IsVisible)
                {
                    if (!this.ToolbarItems.Contains(optionsToolbarItem))
                        this.ToolbarItems.Add(optionsToolbarItem);
                }
                else
                {
                    if (this.ToolbarItems.Contains(optionsToolbarItem))
                        this.ToolbarItems.Remove(optionsToolbarItem);
                }
            }
        }

        private void HandleVendorEditViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IsVisibleProperty.PropertyName)
            {
                if (this.editView?.IsVisible == true)
                {
                    this.detailView.ClosePopup();
                    if (!this.ToolbarItems.Contains(checkToolbarItem))
                        this.ToolbarItems.Add(checkToolbarItem);
                }
                else
                {
                    if (this.ToolbarItems.Contains(checkToolbarItem))
                        this.ToolbarItems.Remove(checkToolbarItem);
                }
            }
        }
    }

    public class PersonDataTemplateSelector : DataTemplateSelector
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

            if (s.Key == "TextBox")
                return TextBoxTemplate;
            else if (s.Key == "MultiLine")
                return MultiLineTemplate;
            else if (s.Key == "DateTime")
                return DateTimeTemplate;
            else if (s.Key == "CheckBox")
                return CheckBoxTemplate;
            else if (s.Key == "ListBoxMulti")
                return ListBoxMultiTemplate;
            else

                return DropDownListTemplate;
            // return ((Person)item).DateOfBirth.Year >= 1980 ? ValidTemplate : InvalidTemplate;
        }
    }
}
