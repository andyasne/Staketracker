using System;
using System.Threading.Tasks;
using Staketracker.Core.Services;
using Staketracker.Core.ViewModels;
using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Root;
using Staketracker.Core.ViewModels.Login;

namespace Staketracker.Core
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }


        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                if (isAuthenticated())
                {
                    await NavigationService.Navigate<RootViewModel>();
                }
                else
                {
                    await NavigationService.Navigate<LoginViewModel>();
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
