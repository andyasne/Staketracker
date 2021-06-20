using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Stakeholders;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.StakeholderList
{
    public partial class StakeholderListPage : MvxContentPage<StakeholdersListViewModel>, IMvxOverridePresentationAttribute
    {
        public StakeholderListPage()
        {
            InitializeComponent();


        }

        private void AddToolbarItems()
        {
            var searchToolbarItem = new ToolbarItem();
            searchToolbarItem.Text = "Search";
            searchToolbarItem.IconImageSource = new FileImageSource() { File = "search" };
            searchToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("SearchCommand"));

            var filterToolbarItem = new ToolbarItem();
            filterToolbarItem.Text = "Filter";
            filterToolbarItem.IconImageSource = new FileImageSource() { File = "Filter" };
            filterToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("OnDevelopmentNotifyCommand"));

            // this.ToolbarItems.Add(searchToolbarItem);
            this.ToolbarItems.Add(filterToolbarItem);
        }

        private async void Handle_RefreshRequested(object sender, Telerik.XamarinForms.DataControls.ListView.PullToRefreshRequestedEventArgs e)
        {
            await ViewModel.Refresh();
            (sender as RadListView).IsPullToRefreshActive = false;
        }


        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxTabbedPagePresentationAttribute(TabbedPosition.Tab) { WrapInNavigationPage = true };
            }
            else
            {
                return new MvxMasterDetailPagePresentationAttribute(MasterDetailPosition.Master) { WrapInNavigationPage = false };
            }
        }

    }
}
