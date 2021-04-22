using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Contacts;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Staketracker Dashboard")]
    public partial class ContactsPage : MvxContentPage<ContactsViewModel>
    {
        public ContactsPage()
        {
            InitializeComponent();

            //  var v = ViewModel.authReply;

        }
    }
}
