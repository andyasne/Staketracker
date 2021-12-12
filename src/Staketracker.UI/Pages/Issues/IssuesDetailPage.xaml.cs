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
using Staketracker.Core.ViewModels.Issues;

namespace Staketracker.UI.Pages.Issues
{

    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class IssuesDetailPage : MvxContentPage<IssuesDetailViewModel>
    {



        public IssuesDetailPage()
        {
            InitializeComponent();



            this.editView.IsVisible = false;
            var trigger = new DataTrigger(editView.GetType());
            trigger.Binding = new Binding("IsEditing");
            trigger.Value = bool.TrueString;
            trigger.Setters.Add(new Setter() { Property = ContentView.IsVisibleProperty, Value = true });
            this.editView.Triggers.Add(trigger);
            //  this.LayoutRoot.Children.Add(editView);

        }

        private ContentView editView;
        private ToolbarItem editToolbarItem, checkToolbarItem, deleteToolbarItem, saveToolbarItem;

        Core.Models.PresentationMode mode = Core.Models.PresentationMode.Create;
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }


        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxContentPagePresentationAttribute() { WrapInNavigationPage = true };
            }
            else
            {
                return new MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail) { NoHistory = true, MasterHostViewType = typeof(IssuesDetailPage) };
            }
        }

    }

}
