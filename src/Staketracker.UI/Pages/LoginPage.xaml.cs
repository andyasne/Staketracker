using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
    public partial class LoginPage : MvxContentPage<LoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //ViewModel.GetDataCommand.Execute(null);
            ViewModel.GetTimeLineDataCommand.Execute(null);


        }
    }
}
