using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.EventsList;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages.EventsList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Staketracker Dashboard")]
    public partial class EventsListPage : MvxContentPage<EventsListViewModel>
    {
        public EventsListPage()
        {
            InitializeComponent();
        }
    }
}
