using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Presenters;
using Staketracker.Core;

namespace Staketracker.Droid
{
    [Activity(

       Label = "@string/app_name",
       Theme = "@style/AppTheme"
     , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : MvxFormsAppCompatActivity<StakeTrackerAndroidSetup, Core.App, UI.App>
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
            Xamarin.Forms.Forms.SetFlags("AppTheme_Experimental");

            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class StakeTrackerAndroidSetup : MvxFormsAndroidSetup<Core.App, UI.App>
    {
        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            return new AppPagePresenter(viewPresenter);
        }
    }
}
