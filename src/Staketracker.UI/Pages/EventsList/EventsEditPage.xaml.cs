using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.EventsList;
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

namespace Staketracker.UI.Pages.EventsList
{
    public partial class EventsEditPage : MvxContentPage<SEventDetailViewModel>, IPopupHost
    {
        private ToolbarItem saveToolbarItem;
        private ToolbarItem deleteToolbarItem;

        public EventsEditPage()
        {
            InitializeComponent();

            saveToolbarItem = new ToolbarItem();
            saveToolbarItem.Text = "Save";
            //     checkToolbarItem.IconImageSource = new FileImageSource() { File = "check" };
            //   checkToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("CommitCommand"));
            Telerik.XamarinForms.Primitives.RadPopup.SetPopup(this, null);


            deleteToolbarItem = new ToolbarItem();
            deleteToolbarItem.Text = "Delete";
            //     checkToolbarItem.IconImageSource = new FileImageSource() { File = "check" };
            //   checkToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("CommitCommand"));

        }
        protected override void OnAppearing()
        {
            this.ToolbarItems.Add(saveToolbarItem);
            this.ToolbarItems.Add(deleteToolbarItem);


        }

        public void ClosePopup()
        {
            this.popup.IsOpen = false;
        }

        public void OpenPopup()
        {
            this.popup.IsOpen = true;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            this.ClosePopup();
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
