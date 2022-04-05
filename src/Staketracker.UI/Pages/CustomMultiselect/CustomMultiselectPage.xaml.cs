using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.CommunicationList;
using Staketracker.Core.ViewModels.Linked.Communication;
using Staketracker.Core.ViewModels.Linked.CustomMultiselect;
using Staketracker.Core.ViewModels.ProjectTeam;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.Linked.CustomMultiselect
{
    public partial class CustomMultiselectPage : MvxContentPage<CustomMultiselectViewModel>
    {
        public CustomMultiselectPage()
        {

            InitializeComponent();

            ToolbarItem doneToolbarItem = new ToolbarItem();

            doneToolbarItem.Text = "Done";

            doneToolbarItem.Clicked += this.doneToolbarItem_Clicked;

            this.ToolbarItems.Add(doneToolbarItem);

        }

        private void doneToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
