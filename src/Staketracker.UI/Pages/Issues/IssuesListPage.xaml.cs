using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.CommunicationList;
using Staketracker.Core.ViewModels.Issues;
using Staketracker.Core.ViewModels.ProjectTeam;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.Issues
{

    public partial class IssuesListPage : MvxContentPage<IssuesListViewModel>
    {
        public IssuesListPage()
        {
            InitializeComponent();

        }


    }
}
