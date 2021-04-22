using MvvmCross.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Staketracker.Core.Services;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, INotifyPropertyChanged
    {

        public IUserDialogs PageDialog = UserDialogs.Instance;
        public IApiManager ApiManager;
        IApiService<IMakeUpApi> makeUpApi = new ApiService<IMakeUpApi>(Config.ApiUrl);
        IApiService<IRedditApi> redditApi = new ApiService<IRedditApi>(Config.RedditApiUrl);
        IApiService<IStaketrackerApi> staketrackerApi = new ApiService<IStaketrackerApi>(Config.StaketrackerApiUrl);
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get; set; }
        public BaseViewModel()
        {
            ApiManager = new ApiManager(makeUpApi, redditApi, staketrackerApi);

        }

        public async Task RunSafe(Task task, bool ShowLoading = true, string loadinMessage = null)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                if (ShowLoading)
                    UserDialogs.Instance.ShowLoading(loadinMessage ?? "Loading");

                await task;
            }
            catch (Exception e)
            {
                IsBusy = false;
                //   UserDialogs.Instance.HideLoading();
                Debug.WriteLine(e.ToString());
                //await Application.Current.MainPage.DisplayAlert("Eror", "Check your internet connection", "Ok");
                await PageDialog.AlertAsync("Check your internet connection", "Error", "Ok");
            }
            finally
            {
                IsBusy = false;
                if (ShowLoading)
                    UserDialogs.Instance.HideLoading();
            }
        }

    }
}
