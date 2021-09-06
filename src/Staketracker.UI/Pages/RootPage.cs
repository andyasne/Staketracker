using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Root;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Detail, NoHistory = false, WrapInNavigationPage = false)]
    //[MvxTabbedPagePresentation(TabbedPosition.Root, WrapInNavigationPage = false, NoHistory = false)]
    public class RootPage : MvxTabbedPage<RootViewModel>
    {
        public RootPage()
        {


            if (Device.Idiom == TargetIdiom.Phone)
            {
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
            }
        }

    }
}
