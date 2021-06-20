using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Root;
using Xamarin.Forms;

namespace Staketracker.UI.Pages
{
    [MvxTabbedPagePresentation(TabbedPosition.Root, WrapInNavigationPage = false, NoHistory = false)]
    public class LoginRootPage : MvxTabbedPage<LoginRootViewModel>
    {
        public LoginRootPage()
        {

            if (Device.Idiom == TargetIdiom.Phone)
            {
                // Maximum number of items supported by BottomNavigationView is 5.
                Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.SetToolbarPlacement(this,
                    Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);

            }
        }
    }
}

