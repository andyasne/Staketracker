using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.ForgetPassword;
using Staketracker.Core.ViewModels.ForgetUserId;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms;

namespace Staketracker.UI.Pages
{
    public partial class ForgetPasswordPage : MvxContentPage<ForgetPasswordViewModel>, IMvxOverridePresentationAttribute
    {
        public ForgetPasswordPage()
        {
            InitializeComponent();



        }


        private void isSandbox_IsCheckedChanged(object sender, Telerik.XamarinForms.Primitives.CheckBox.IsCheckedChangedEventArgs e)
        {

        }


        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxTabbedPagePresentationAttribute(TabbedPosition.Tab) { WrapInNavigationPage = true };
            }
            else
            {
                return new MvxTabbedPagePresentationAttribute(TabbedPosition.Root) { WrapInNavigationPage = false };
            }
        }

    }
}