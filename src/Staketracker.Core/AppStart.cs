using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Root;
using System;
using System.Threading.Tasks;
using Staketracker.Core.ViewModels.Menu;

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
