using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Communication;
using System;
using Staketracker.UI.Pages.Communication;
using Xamarin.Forms;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core;

namespace Staketracker.UI.Pages.CommunicationsDetail
{

    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class CommunicationDetailPage : MvxContentPage<CommunicationDetailViewModel>
    {



        public CommunicationDetailPage()
        {
            InitializeComponent();

            if (Device.Idiom == TargetIdiom.Phone)
            {
                this.editView = new CommunicationEditView();
                this.detailView.PropertyChanged += this.HandleCustomerDetailViewPropertyChanged;
                this.editView.PropertyChanged += this.HandleCustomerEditViewPropertyChanged;

                editToolbarItem = new ToolbarItem();
                editToolbarItem.Text = "Edit";
                editToolbarItem.IconImageSource = new FileImageSource() { File = "Edit" };
                editToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("BeginEditCommand"));


                saveToolbarItem = new ToolbarItem();
                saveToolbarItem.Text = "Save";
                saveToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("SaveCommand"));
                //    saveToolbarItem.Clicked += this.editToolbarItem_Clicked;


                deleteToolbarItem = new ToolbarItem();
                deleteToolbarItem.Text = "Delete";
                deleteToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("DeleteCommand"));


            }
            else
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }

            this.editView.IsVisible = false;
            var trigger = new DataTrigger(editView.GetType());
            trigger.Binding = new Binding("IsEditing");
            trigger.Value = bool.TrueString;
            trigger.Setters.Add(new Setter() { Property = ContentView.IsVisibleProperty, Value = true });
            this.editView.Triggers.Add(trigger);
            this.LayoutRoot.Children.Add(editView);
        }

        private ContentView editView;
        private ToolbarItem editToolbarItem, checkToolbarItem, deleteToolbarItem, saveToolbarItem;



        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.Idiom != TargetIdiom.Phone)
                return;

            if (this.detailView.IsVisible)
            {
                if (!this.ToolbarItems.Contains(editToolbarItem))
                {
                    this.ToolbarItems.Add(editToolbarItem);
                    this.ToolbarItems.Add(deleteToolbarItem);
                }

            }

            if (this.editView.IsVisible)
            {
                if (!this.ToolbarItems.Contains(saveToolbarItem))
                {
                    this.ToolbarItems.Add(saveToolbarItem);
                }
            }
        }
        private void editToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            if (this.editView.IsVisible)
                ((IPopupHost)this.editView).OpenPopup();
        }

        private void HandleCustomerDetailViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IsVisibleProperty.PropertyName)
            {
                if (this.detailView.IsVisible)
                {
                    if (!this.ToolbarItems.Contains(editToolbarItem))
                    {
                        this.ToolbarItems.Add(editToolbarItem);
                        this.ToolbarItems.Add(deleteToolbarItem);
                    }

                }
                else
                {
                    if (this.ToolbarItems.Contains(editToolbarItem))
                    {
                        this.ToolbarItems.Remove(editToolbarItem);
                        this.ToolbarItems.Remove(deleteToolbarItem);
                    }

                }
            }
        }

        private void HandleCustomerEditViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IsVisibleProperty.PropertyName)
            {
                if (this.editView?.IsVisible == true)
                {
                    if (!this.ToolbarItems.Contains(saveToolbarItem))
                    {
                        this.ToolbarItems.Add(saveToolbarItem);
                    }
                }
                else
                {
                    if (this.ToolbarItems.Contains(saveToolbarItem))
                    {
                        this.ToolbarItems.Remove(saveToolbarItem);
                    }
                }
            }
        }
        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return new MvxContentPagePresentationAttribute() { WrapInNavigationPage = true };
            }
            else
            {
                return new MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail) { NoHistory = true, MasterHostViewType = typeof(CommunicationDetailPage) };
            }
        }

    }

}
