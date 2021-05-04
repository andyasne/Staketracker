using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Root;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Root, WrapInNavigationPage = false)]
    // [MvxTabbedPagePresentation(TabbedPosition.Root, WrapInNavigationPage = false, NoHistory = false)]

    public partial class RootPage : MvxMasterDetailPage<RootViewModel>
    {
        public RootPage()
        {
            InitializeComponent();
        }
    }
}
