using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.Models.Stakeholders;

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

        Task<HttpResponseMessage> AddEvent(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> AddStakeholder(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> AddCommunication(jsonTextObj jsonTextObj, string sessionId);

        Task<HttpResponseMessage> GetGroupStakeholderDetails(jsonTextObj jsonTextObj, string sessionId);


        Task<HttpResponseMessage> GetIndividualStakeholderDetails(jsonTextObj jsonTextObj, string sessionId);


        Task<HttpResponseMessage> GetLandParcelStakeholderDetails(jsonTextObj jsonTextObj, string sessionId);

        Task<HttpResponseMessage> GetAllStakeholders(jsonTextObj jsonTextObj, string sessionId);

        Task<HttpResponseMessage> GetCommunicationDetails(jsonTextObj jsonTextObj, string sessionId);

        Task<HttpResponseMessage> ChangePassword(jsonTextObj jsonTextObj, string sessionId);

        Task<HttpResponseMessage> GetProjectTeam(jsonTextObj jsonTextObj, string sessionId);

        Task<HttpResponseMessage> GetIssues(jsonTextObj jsonTextObj, string sessionId);

        Task<HttpResponseMessage> GetIssueDetails(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> GetProjectTeamMemberDetails(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> DelRec(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> RequestUsr(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> RequestPwd(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> GetProjectList(jsonTextObj jsonTextObj, string sessionId);
        Task<HttpResponseMessage> SwitchProject(jsonTextObj jsonTextObj, string sessionId);


    }
}
