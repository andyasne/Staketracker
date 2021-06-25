using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Root;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{
    [MvxTabbedPagePresentation(TabbedPosition.Root, WrapInNavigationPage = false, NoHistory = false)]
    public class RootPage : MvxTabbedPage<RootViewModel>
    {
        public RootPage()
        {


            if (Device.Idiom == TargetIdiom.Phone)
            {
                // Maximum number of items supported by BottomNavigationView is 5.
                Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.SetToolbarPlacement(this,
                    Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);
            }


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
                //   navigationPage.BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            }
        }

    }
}
