using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.CommunicationList;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms.Xaml;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms;
using System.Collections.Generic;
using Staketracker.Core.Validators;

namespace Staketracker.UI.Pages.CommunicationList
{


    public class PersonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextBoxTemplate { get; set; }
        public DataTemplate DropDownListTemplate { get; set; }
        public DataTemplate MultiLineTemplate { get; set; }
        public DataTemplate DateTimeTemplate { get; set; }
        public DataTemplate CheckBoxTemplate { get; set; }
        public DataTemplate ListBoxMultiTemplate { get; set; }




        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var s = (KeyValuePair<string, ValidatableObject<string>>)item;
            var k = s.Value.FormAndDropDownField.InputType;
            if (k == "TextBox")
                return TextBoxTemplate;
            else if (k == "MultiLine")
                return MultiLineTemplate;
            else if (k == "DateTime")
                return DateTimeTemplate;
            else if (k == "CheckBox")
                return CheckBoxTemplate;
            else if (k == "ListBoxMulti")
                return ListBoxMultiTemplate;
            else

                return DropDownListTemplate;
            // return ((Person)item).DateOfBirth.Year >= 1980 ? ValidTemplate : InvalidTemplate;
        }
    }
    public partial class CommunicationListPage : MvxContentPage<CommunicationListViewModel>, IMvxOverridePresentationAttribute
    {
        public CommunicationListPage()
        {
            InitializeComponent();


            //var service = MvvmCross.Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            //var vm = service.LoadViewModel(new MvxViewModelInstanceRequest(typeof(CommunicationListViewModel)), null);
            //ViewModel = vm as CommunicationListViewModel;


            AddToolbarItems();

        }

        private void AddToolbarItems()
        {
            var searchToolbarItem = new ToolbarItem();
            searchToolbarItem.Text = "Search";
            searchToolbarItem.IconImageSource = new FileImageSource() { File = "search" };
            searchToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("SearchCommand"));

            var filterToolbarItem = new ToolbarItem();
            filterToolbarItem.Text = "Filter";
            filterToolbarItem.IconImageSource = new FileImageSource() { File = "Filter" };
            filterToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("OnDevelopmentNotifyCommand"));


            this.ToolbarItems.Add(searchToolbarItem);
            this.ToolbarItems.Add(filterToolbarItem);
        }

        private async void Handle_RefreshRequested(object sender, Telerik.XamarinForms.DataControls.ListView.PullToRefreshRequestedEventArgs e)
        {
            await ViewModel.Refresh();
            (sender as RadListView).IsPullToRefreshActive = false;
        }


        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxTabbedPagePresentationAttribute(TabbedPosition.Tab) { WrapInNavigationPage = true };
            }
            else
            {
                return new MvxMasterDetailPagePresentationAttribute(MasterDetailPosition.Master) { WrapInNavigationPage = false };
            }
        }

    }
}
