using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels;
using Xamarin.Forms.Xaml;


namespace Staketracker.UI.Pages
{

    //[XamlCompilation(XamlCompilationOptions.Compile)]
    //[MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class SearchResultsPage : MvxContentPage<SearchResultsViewModel>
    {

        public SearchResultsPage()
        {

            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(100);
            searchBox.Focus();
        }
    }
}
