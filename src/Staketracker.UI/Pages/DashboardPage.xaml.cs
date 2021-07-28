using Java.Lang;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Dashboard;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Thread = System.Threading.Thread;

namespace Staketracker.UI.Pages
{
    public partial class DashboardPage : MvxContentPage<DashboardViewModel>, IMvxOverridePresentationAttribute
    {
        public DashboardPage()
        {
            InitializeComponent();

            CultureInfo info = new CultureInfo("fr");
            Thread.CurrentThread.CurrentUICulture = info;


        }



        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            //if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxTabbedPagePresentationAttribute(TabbedPosition.Tab) { WrapInNavigationPage = true };
            }
            //else
            //{
            //    return new MvxTabbedPagePresentationAttribute(TabbedPosition.Tab) { WrapInNavigationPage = false };
            //}
        }
    }


}
