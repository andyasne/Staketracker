using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Stakeholders;
using System;
using Staketracker.Core.ViewModels.Stakeholder;
using Staketracker.UI.Pages.StakeholderList;
using Staketracker.UI.Pages.stakeholders;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.StakeholdersDetail
{

    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class StakeholderDetailPage : MvxContentPage<StakeholderDetailViewModel>
    {
        public StakeholderDetailPage()
        {
            InitializeComponent();

            if (Device.Idiom == TargetIdiom.Phone)
            {
                // this.detailView.PropertyChanged += this.HandleVendorDetailViewPropertyChanged;
                this.editView = new StakeholderDetailView();
                ///  this.editView.PropertyChanged += this.HandleVendorEditViewPropertyChanged;

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

                // NavigationPage.SetHasNavigationBar(this, false);
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
            //    this.detailView.OpenPopup();
        }


    }

}
