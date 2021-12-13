using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Communication;
using System;
using Staketracker.UI.Pages.Communication;
using Xamarin.Forms;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core;
using Staketracker.Core.Res;
using Staketracker.Core.ViewModels.ProjectTeam;

namespace Staketracker.UI.Pages.ProjectTeam
{

    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class ProjectTeamDetailPage : MvxContentPage<ProjectTeamDetailViewModel>
    {



        public ProjectTeamDetailPage()
        {
            InitializeComponent();



        }


        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxContentPagePresentationAttribute() { WrapInNavigationPage = true };
            }
            else
            {
                return new MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail) { NoHistory = true, MasterHostViewType = typeof(ProjectTeamDetailPage) };
            }
        }

    }

}
