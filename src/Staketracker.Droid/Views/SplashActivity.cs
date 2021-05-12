using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using MvvmCross.Forms.Platforms.Android.Views;

namespace Staketracker.Droid.Views
{

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
