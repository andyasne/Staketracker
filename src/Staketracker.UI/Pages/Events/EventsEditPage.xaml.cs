using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Events;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms.Xaml;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms;
using Staketracker.Core;

namespace Staketracker.UI.Pages.Events
{
    public partial class EventsEditPage : ContentView
    {
        private ToolbarItem saveToolbarItem;
        private ToolbarItem deleteToolbarItem;

        public EventsEditPage()
        {
            InitializeComponent();

            saveToolbarItem = new ToolbarItem();
            saveToolbarItem.Text = "Save";
            //     checkToolbarItem.IconImageSource = new FileImageSource() { File = "check" };
            saveToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("CommitCommand"));


            deleteToolbarItem = new ToolbarItem();
            deleteToolbarItem.Text = "Delete";
            //     checkToolbarItem.IconImageSource = new FileImageSource() { File = "check" };
            deleteToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("DeleteCommand"));

        }


        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxContentPagePresentationAttribute() { WrapInNavigationPage = true };
            }
            else
            {
                return new MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail) { NoHistory = true, MasterHostViewType = typeof(SEventsListPage) };
            }
        }
    }
}
