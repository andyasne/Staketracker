using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;

namespace Staketracker.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, Core.App, UI.App>
    {
    }

    public class Setup : MvxFormsIosSetup<Core.App, UI.App>
    {
    }
}
