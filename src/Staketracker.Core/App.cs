using System.Globalization;
using System.Threading;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Staketracker.Core.Res;
//using Staketracker.Core.Resources;

namespace Staketracker.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();



            RegisterCustomAppStart<AppStart>();
        }
    }
}
