using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail)]

    public partial class LoginPage : MvxContentPage<LoginViewModel>, IMvxOverridePresentationAttribute
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
            }
        }
        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {

            return new MvxMasterDetailPagePresentationAttribute() { WrapInNavigationPage = true, NoHistory = true };

        }

    }
}
