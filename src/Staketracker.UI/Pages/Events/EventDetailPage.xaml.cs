using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Staketracker.Core;
using Staketracker.Core.ViewModels.Events;
using System;
using Xamarin.Forms;

namespace Staketracker.UI.Pages.Events
{

    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class EventDetailPage : MvxContentPage<SEventDetailViewModel>
    {
        public EventDetailPage()
        {
            InitializeComponent();

            if (Device.Idiom == TargetIdiom.Phone)
            {
                this.editView = new EventEditView();
                this.detailView.PropertyChanged += this.HandleCustomerDetailViewPropertyChanged;
                this.editView.PropertyChanged += this.HandleCustomerEditViewPropertyChanged;

                optionsToolbarItem = new ToolbarItem();
                optionsToolbarItem.Text = "Edit";
                optionsToolbarItem.IconImageSource = new FileImageSource() { File = "Edit" };
                optionsToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("BeginEditCommand"));


                saveToolbarItem = new ToolbarItem();
                saveToolbarItem.Text = "Save";
                saveToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("SaveCommand"));

                deleteToolbarItem = new ToolbarItem();
                deleteToolbarItem.Text = "Delete";
                deleteToolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("DeleteCommand"));

                //  checkToolbarItem.Clicked += this.OptionsToolbarItem_Clicked;


            }
            else
            {
                //this.editView = new VendorEditViewTablet();

                NavigationPage.SetHasNavigationBar(this, false);
            }
            this.ToolbarItems.Add(saveToolbarItem);
            this.ToolbarItems.Add(deleteToolbarItem);

            this.editView.IsVisible = false;
            var trigger = new DataTrigger(editView.GetType());
            trigger.Binding = new Binding("IsEditing");
            trigger.Value = bool.TrueString;
            trigger.Setters.Add(new Setter() { Property = ContentView.IsVisibleProperty, Value = true });
            this.editView.Triggers.Add(trigger);
            this.LayoutRoot.Children.Add(editView);
        }

        private ContentView editView;
        private ToolbarItem optionsToolbarItem, checkToolbarItem, deleteToolbarItem, saveToolbarItem;



        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.Idiom != TargetIdiom.Phone)
                return;

            if (this.detailView.IsVisible)
            {
                if (!this.ToolbarItems.Contains(optionsToolbarItem))
                    this.ToolbarItems.Add(optionsToolbarItem);
            }

            if (this.editView.IsVisible)
            {
                if (!this.ToolbarItems.Contains(saveToolbarItem))
                {
                    this.ToolbarItems.Add(saveToolbarItem);
                    this.ToolbarItems.Add(deleteToolbarItem);
                }
            }
        }

        private void OptionsToolbarItem_Clicked(object sender, System.EventArgs e)
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
                    if (!this.ToolbarItems.Contains(optionsToolbarItem))
                        this.ToolbarItems.Add(optionsToolbarItem);
                }
                else
                {
                    if (this.ToolbarItems.Contains(optionsToolbarItem))
                        this.ToolbarItems.Remove(optionsToolbarItem);
                }
            }
        }

        private void HandleCustomerEditViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IsVisibleProperty.PropertyName)
            {
                if (this.editView?.IsVisible == true)
                {
                    this.detailView.ClosePopup();
                    if (!this.ToolbarItems.Contains(saveToolbarItem))
                    {
                        this.ToolbarItems.Add(saveToolbarItem);
                        this.ToolbarItems.Add(deleteToolbarItem);
                    }
                }
                else
                {
                    if (this.ToolbarItems.Contains(saveToolbarItem))
                    {
                        this.ToolbarItems.Remove(saveToolbarItem);
                        this.ToolbarItems.Remove(deleteToolbarItem);
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
                return new MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition.Detail) { NoHistory = true, MasterHostViewType = typeof(EventDetailPage) };
            }
        }

    }

}
