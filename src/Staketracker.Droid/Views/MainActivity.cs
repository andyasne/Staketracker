using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Presenters;
using Staketracker.Core;
using Staketracker.Core.ViewModels.Main;

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

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            //Xamarin.Forms.Forms.Init(this, bundle);
            //  DisplayCrashReport();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Xamarin.Forms.Forms.SetFlags("AppTheme_Experimental");

            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
        }


        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}‪


        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = string.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) =>
                {
                    // User pressed Close.
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }




        // In MainActivity
        //protected override void OnCreate(Bundle bundle)
        //{
        //    base.OnCreate(bundle);

        //    AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        //    TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

        //    Xamarin.Forms.Forms.Init(this, bundle);
        //    DisplayCrashReport();

        //    var app = new App();
        //    LoadApplication(app);
        //}  



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
