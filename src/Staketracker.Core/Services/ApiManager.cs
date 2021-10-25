using Acr.UserDialogs;
using Fusillade;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Polly;
using Refit;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.Models.Stakeholders;

namespace Staketracker.Core.Services
{
    public class ApiManager : IApiManager
    {
        private IUserDialogs _userDialogs = UserDialogs.Instance;
        private IConnectivity _connectivity = CrossConnectivity.Current;

        private IApiService<IStaketrackerApi> staketrackerApi;
        public bool IsConnected { get; set; }
        public bool IsReachable { get; set; }
        private Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();
        private Dictionary<string, Task<HttpResponseMessage>> taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();

        public ApiManager(IApiService<IStaketrackerApi> _staketrackerApi)
        {
            staketrackerApi = _staketrackerApi;
            IsConnected = _connectivity.IsConnected;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            IsConnected = e.IsConnected;

            if (!e.IsConnected)
            {
                // Cancel All Running Task
                var items = runningTasks.ToList();
                foreach (var item in items)
                {
                    item.Value.Cancel();
                    runningTasks.Remove(item.Key);
                }
            }
        }

        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            TData data = new TData();

            if (!IsConnected)
            {
                var strngResponse = "There's not a network connection";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);

                _userDialogs.Toast(strngResponse, TimeSpan.FromSeconds(1));
                return data;
            }
            else
            {
                // _userDialogs.Toast("Connection Established With sustainet.com Server", TimeSpan.FromSeconds(3));
            }

            IsReachable = await _connectivity.IsRemoteReachable(Config.ApiHostName);

            if (!IsReachable)
            {
                var strngResponse = "There's not an internet connection";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);

                _userDialogs.Toast(strngResponse, TimeSpan.FromSeconds(1));
                return data;
            }

            data = await Policy
            .Handle<WebException>()
            .Or<ApiException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync
            (
                retryCount: 1,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            )
            .ExecuteAsync(async () =>
            {
                var result = await task;

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //Logout the user
                }
                runningTasks.Remove(task.Id);

                return result;
            });

            return data;
        }
        private async Task<HttpResponseMessage> AddToRunningTasks(CancellationTokenSource cts, Task<HttpResponseMessage> task)
        {
            runningTasks.Add(task.Id, cts);
            return await task;
        }

        public async Task<HttpResponseMessage> AuthenticateUser(LoginAPIBody loginApiBody)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).AuthenticateUser(loginApiBody, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> Is2FEnabled(LoginAPIBody loginApiBody)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).Is2FEnabled(loginApiBody, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> GetUsrEmail(LoginAPIBody loginApiBody)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetUsrEmail(loginApiBody, cts.Token));
            return await AddToRunningTasks(cts, task);
        }


        public async Task<HttpResponseMessage> GetEvents(APIRequestBody aPIRequestBody, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetEvents(aPIRequestBody, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> GetFormAndDropDownFieldValues(FormFieldBody formFieldBody, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetFormAndDropDownFieldValues(formFieldBody, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> GetEventDetails(APIRequestExtraBody aPIRequestExtraBody, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetEventDetails(aPIRequestExtraBody, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }


        public async Task<HttpResponseMessage> GetAllCommunications(APIRequestBody aPIRequestBody, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetAllCommunications(aPIRequestBody, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> AddEvent(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).AddEvent(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> AddStakeholder(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).AddStakeholder(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> AddCommunication(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).AddCommunication(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> GetGroupStakeholderDetails(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetGroupStakeholderDetails(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }
        public async Task<HttpResponseMessage> GetIndividualStakeholderDetails(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetIndividualStakeholderDetails(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }
        public async Task<HttpResponseMessage> GetLandParcelStakeholderDetails(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetLandParcelStakeholderDetails(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> GetAllStakeholders(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetAllStakeholders(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

        public async Task<HttpResponseMessage> GetCommunicationDetails(jsonTextObj jsonTextObj, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).GetCommunicationDetails(jsonTextObj, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordBody changePasswordBody, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(staketrackerApi.GetApi(Priority.UserInitiated).ChangePassword(changePasswordBody, sessionId, cts.Token));
            return await AddToRunningTasks(cts, task);

        }

    }
}
