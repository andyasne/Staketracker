using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.EventsList;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms.Xaml;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.EventsList
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    //[MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "SustaiNet Core")]
    public partial class EventsListPage : MvxContentPage<EventsListViewModel>, IMvxOverridePresentationAttribute
    {
        public EventsListPage()
        {
            InitializeComponent();

            this.IconImageSource = new FileImageSource() { File = "search" };
            this.LayoutRoot.Children.Remove(this.searchBar);
            this.LayoutRoot.RowDefinitions.Clear();
            this.ContentRoot.ClearValue(Grid.RowProperty);

            var searchToolbarItem = new ToolbarItem();
            searchToolbarItem.Text = "Search";
            searchToolbarItem.IconImageSource = new FileImageSource() { File = "search" };
            searchToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("SearchCommand"));



            this.ToolbarItems.Add(searchToolbarItem);


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
