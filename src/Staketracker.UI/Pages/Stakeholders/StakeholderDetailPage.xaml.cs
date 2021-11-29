using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels.Stakeholders;
using System;
using Staketracker.Core.ViewModels.Stakeholder;
using Staketracker.UI.Pages.StakeholderList;
using Staketracker.UI.Pages.stakeholders;
using Xamarin.Forms;
using Staketracker.UI.Pages.Stakeholder;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core;

namespace Staketracker.UI.Pages.StakeholdersDetail
{

    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class StakeholderDetailPage : MvxContentPage<StakeholderDetailViewModel>
    {
        public StakeholderDetailPage()
        {
            InitializeComponent();

            if (Device.Idiom == TargetIdiom.Phone)
            {
                this.editView = new StakeholderEditView();
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

        Core.Models.PresentationMode mode = Core.Models.PresentationMode.Create;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
            {
                mode = ((StakeholderDetailViewModel)ViewModel).Mode;
            }

            if (this.detailView.IsVisible && mode == Core.Models.PresentationMode.Read)
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

            if (mode == Core.Models.PresentationMode.Create)
            {
                this.ToolbarItems.Clear();
                this.ToolbarItems.Add(saveToolbarItem);

            }
        }

        private void HandleCustomerDetailViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                mode = ((StakeholderDetailViewModel)ViewModel).Mode;
            }

            if (e.PropertyName == IsVisibleProperty.PropertyName)
            {
                this.ToolbarItems.Clear();

                if (this.detailView.IsVisible)
                {
                    if (!this.ToolbarItems.Contains(editToolbarItem) && mode == Core.Models.PresentationMode.Read)
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
            if (mode == Core.Models.PresentationMode.Create)
            {
                this.ToolbarItems.Clear();
                this.ToolbarItems.Add(saveToolbarItem);

            }
        }
        private void HandleCustomerEditViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                mode = ((StakeholderDetailViewModel)ViewModel).Mode;
            }
            if (e.PropertyName == IsVisibleProperty.PropertyName)
            {
                this.ToolbarItems.Clear();

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
            if (mode == Core.Models.PresentationMode.Create)
            {
                this.ToolbarItems.Clear();
                this.ToolbarItems.Add(saveToolbarItem);

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
                return new MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail) { NoHistory = true, MasterHostViewType = typeof(StakeholderDetailPage) };
            }
        }

    }

}
