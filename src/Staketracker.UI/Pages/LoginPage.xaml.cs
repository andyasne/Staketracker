using System.ComponentModel;
using System.Globalization;
using System.Threading;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Login;
using Staketracker.Core.Resources;
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
            //CultureInfo language = new CultureInfo("am");
            //Thread.CurrentThread.CurrentUICulture = language;
            //Resource1.Culture = language;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
            }
        }

        void UltimateEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is PhantomLib.CustomControls.UltimateEntry entry)
            {
                //    entry.ShowError = e.NewTextValue.Length > 4;
            }
        }
        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {

            return new MvxMasterDetailPagePresentationAttribute() { WrapInNavigationPage = true, NoHistory = true };

        }

    }

    public class ShowPasswordTriggerAction : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        bool _hidePassword = true;

        public bool HidePassword
        {
            set
            {
                if (_hidePassword != value)
                {
                    _hidePassword = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
            }
            get => _hidePassword;
        }

        protected override void Invoke(ImageButton sender)
        {
            sender.Source = HidePassword ? ShowIcon : HideIcon;
            HidePassword = !HidePassword;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
