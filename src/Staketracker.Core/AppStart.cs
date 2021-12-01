using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Root;
using System;
using System.Threading.Tasks;
using Staketracker.Core.ViewModels.Menu;
using Staketracker.Core.ViewModels.Login;
using System.Globalization;
using System.Threading;
using Staketracker.Core.Res;
using Plugin.Settings;

namespace Staketracker.Core
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
            string defaultLang = CrossSettings.Current.GetValueOrDefault("DefaultLanguage", "en");
            CultureInfo language = new CultureInfo(defaultLang);
            Thread.CurrentThread.CurrentUICulture = language;
            AppRes.Culture = language;
        }


        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                if (isAuthenticated())
                {
                    //   await NavigationService.Navigate<MenuViewModel>();
                    await NavigationService.Navigate<RootViewModel>();
                }
                else
                {
                    await NavigationService.Navigate<LoginRootViewModel>();
                }
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap();
            }
        }
        bool isAuthenticated()
        {
            //Do Auth Impl Here
            return false;

        }
    }
}
