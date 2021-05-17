using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Views;
using Android.Support.V7.App;
using Android.Content.PM;

namespace Staketracker.Droid.Views
{
    [Activity(Theme = "@style/AppTheme.Splash", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
            this.Finish();
        }
    }
}
