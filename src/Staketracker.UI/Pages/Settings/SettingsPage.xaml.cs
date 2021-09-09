using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.ForgetPassword;
using Staketracker.Core.ViewModels.ForgetUserId;
using Staketracker.Core.ViewModels.Login;
using Staketracker.Core.ViewModels.Settings;
using Xamarin.Forms;

namespace Staketracker.UI.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]

    public partial class SettingsPage : MvxContentPage<SettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();

        }

    }
}
