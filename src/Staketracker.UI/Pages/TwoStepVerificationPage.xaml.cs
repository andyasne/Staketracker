using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.TwoStepVerification;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
    public partial class TwoStepVerificationPage : MvxContentPage<TwoStepVerificationViewModel>
    {
        public TwoStepVerificationPage()
        {
            InitializeComponent();
        }
    }
}
