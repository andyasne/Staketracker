using Acr.UserDialogs;
using Android.App;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Views;
using Staketracker.Core.ViewModels.Main;

namespace Staketracker.Droid
{
    [Activity(
        Theme = "@style/AppTheme")]
    public class MainActivity : MvxFormsAppCompatActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                UserDialogs.Init(this);

            }
            catch (System.Exception)
            {

            }
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
        }
    }
}
