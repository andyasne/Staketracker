using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Staketracker.Core.Models.EventsFormValue;

namespace Staketracker.Core.Services
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> AuthenticateUser(LoginAPIBody loginApiBody);

        Task<HttpResponseMessage> Is2FEnabled(LoginAPIBody loginApiBody);

        Task<HttpResponseMessage> GetUsrEmail(LoginAPIBody loginApiBody);

        Task<HttpResponseMessage> GetEvents(APIRequestBody apiRequestBody, string sessionId);

        Task<HttpResponseMessage> GetFormAndDropDownFieldValues(FormFieldBody formFieldBody, string sessionId);

        Task<HttpResponseMessage> GetEventDetails(APIRequestExtraBody aPIRequestExtraBody, string sessionId);

        Task<HttpResponseMessage> GetAllCommunications(APIRequestBody apiRequestBody, string sessionId);

        Task<HttpResponseMessage> AddEvent(EventFormValue eventFormValue, string sessionId);





    }
}
