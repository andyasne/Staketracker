using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.ForgetUserId;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms;

namespace Staketracker.UI.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class ForgetUserIdPage : MvxContentPage<ForgetUserIdViewModel>
    {
        public ForgetUserIdPage()
        {
            InitializeComponent();



        }



    }
}
