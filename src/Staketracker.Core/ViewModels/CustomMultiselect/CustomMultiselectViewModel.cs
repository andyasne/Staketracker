using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Communication;
using Staketracker.Core.Models.Events;
using Xamarin.Forms;
using System.Linq;
using Staketracker.Core.ViewModels.Communication;
using Staketracker.Core.ViewModels.Events;
using D = Staketracker.Core.Models.Events.D;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Res;
using System;
using Staketracker.Core.Models.LinkedTo;
using System.Windows.Input;
using Staketracker.Core.Models.EventsFormValue;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Staketracker.Core.ViewModels.Linked.CustomMultiselect
{

    public class Communication
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class CustomMultiselectViewModel : BaseViewModel<AuthReply>

    {


        private readonly IMvxNavigationService _navigationService;

        public CustomMultiselectViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            Open = new MvxCommand(open);

        }

        private void open()
        {

        }

        public override async void Prepare(AuthReply parameter)
        {

            this.authReply = parameter;
            base.Prepare();


        }

        private ObservableCollection<MultiSelectModel> multiSelectModels = new ObservableCollection<MultiSelectModel>();
        public ObservableCollection<MultiSelectModel> CustomMultiselectRecordList
        {
            get => multiSelectModels;
            set
            {
                SetField(ref multiSelectModels, value);

            }
        }



        private ObservableCollection<object> selectedItems = new ObservableCollection<object>();

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return this.selectedItems;
            }
            set
            {
                if (this.selectedItems != value)
                {
                    if (this.selectedItems != null)
                    {
                        //   this.selectedItems.CollectionChanged -= this.OnSelectedItemsCollectionChanged;
                    }

                    this.selectedItems = value;

                    if (this.selectedItems != null)
                    {
                        //    this.selectedItems.CollectionChanged += this.OnSelectedItemsCollectionChanged;
                    }

                    OnPropertyChanged("selectedItems");
                }
            }
        }

        //private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    var action = e.Action;
        //    if (action == NotifyCollectionChangedAction.Add)
        //    {
        //        this.isInternalCheckChanged = true;
        //        if (this.SelectedItems.Count == this.Items.Count)
        //        {
        //            this.SelectAllChecked = true;
        //        }
        //        else
        //        {
        //            this.SelectAllChecked = null;
        //        }
        //        this.isInternalCheckChanged = false;

        //        return;
        //    }

        //    if (action == NotifyCollectionChangedAction.Remove)
        //    {
        //        this.isInternalCheckChanged = true;
        //        if (this.SelectedItems.Count == 0)
        //        {
        //            this.SelectAllChecked = false;
        //        }
        //        else
        //        {
        //            this.SelectAllChecked = null;
        //        }
        //        this.isInternalCheckChanged = false;
        //    }
        //}



        public ICommand Open { get; set; }


        internal async Task GetEvents(AuthReply authReply)
        {
            EventsReply EventsReply_;

            var apiReq = new APIRequestBody(authReply);
            HttpResponseMessage events = await ApiManager.GetEvents(apiReq, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                EventsReply_ = await Task.Run(() => JsonConvert.DeserializeObject<EventsReply>(response));

                CreateControlModels(EventsReply_);

            }
            else
                await PageDialog.AlertAsync("API Error While retrieving Events", "API Response Error", "Ok");

        }

        private void CreateControlModels(EventsReply EventsReply_)
        {
            int index = 0;

            foreach (Staketracker.Core.Models.Events.D eventReply in EventsReply_.d)
            {
                MultiSelectModel multiSelectObj = new MultiSelectModel(index, eventReply.Name, false, EventsReply_);
                CustomMultiselectRecordList.Add(multiSelectObj);
                index++;
            }
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            RunSafe(GetEvents(authReply), true, "Loading");



        }

    }
}
