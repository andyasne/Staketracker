using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Events;
using System;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.Events
{

    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class EventDetailPage : MvxContentPage<SEventDetailViewModel>
    {
        public EventDetailPage()
        {
            InitializeComponent();

            if (Device.Idiom == TargetIdiom.Phone)
            {
                this.editView = new EventDetailView();


                optionsToolbarItem = new ToolbarItem();
                optionsToolbarItem.Text = "Layout";
                optionsToolbarItem.IconImageSource = new FileImageSource() { File = "ellipsis" };
                optionsToolbarItem.Clicked += this.OptionsToolbarItem_Clicked;

                checkToolbarItem = new ToolbarItem();
                checkToolbarItem.Text = "Save";
                checkToolbarItem.IconImageSource = new FileImageSource() { File = "check" };
                checkToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("CommitCommand"));

            }
            else
            {
                //this.editView = new VendorEditViewTablet();

                NavigationPage.SetHasNavigationBar(this, false);
            }
            this.ToolbarItems.Add(checkToolbarItem);
            //   this.ToolbarItems.Add(deleteToolbarItem);

            this.editView.IsVisible = false;
            var trigger = new DataTrigger(editView.GetType());
            trigger.Binding = new Binding("IsEditing");
            trigger.Value = bool.TrueString;
            trigger.Setters.Add(new Setter() { Property = ContentView.IsVisibleProperty, Value = true });
            this.editView.Triggers.Add(trigger);
            this.LayoutRoot.Children.Add(editView);
        }

        private ContentView editView;
        private ToolbarItem optionsToolbarItem, checkToolbarItem, deleteToolbarItem;


        private void OptionsToolbarItem_Clicked(object sender, EventArgs e)
        {
            this.detailView.OpenPopup();
        }


    }

}
