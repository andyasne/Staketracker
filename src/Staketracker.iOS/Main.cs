using UIKit;

namespace Staketracker.iOS
{
    public static class Application
    {
        private static void Main(string[] args)
        {
            try
            {

               UIApplication.Main(args, null, nameof(AppDelegate));
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
