using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Home;
using Staketracker.Core.ViewModels.Root;

namespace Staketracker.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            //CreatableTypes()
            //    .EndingWith("Service")
            //    .AsInterfaces()
            //    .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }
    }
}
