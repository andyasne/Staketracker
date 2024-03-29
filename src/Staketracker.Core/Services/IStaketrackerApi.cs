using Refit;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.Models.Stakeholders;

namespace Staketracker.Core
{
    [Headers("Content-Type: application/json")]
    public interface IStaketrackerApi
    {
        [Post("/sandbox/mobilev1.asmx/authenticateUser")]
        Task<HttpResponseMessage> AuthenticateUser([Body] LoginAPIBody loginApiBody, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/Is2FEnabled")]
        Task<HttpResponseMessage> Is2FEnabled([Body] LoginAPIBody loginApiBody, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/getUsrEmail")]
        Task<HttpResponseMessage> GetUsrEmail([Body] LoginAPIBody loginApiBody, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getEvents")]
        Task<HttpResponseMessage> GetEvents([Body] APIRequestBody aPIRequestBody, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/addEvent")]
        Task<HttpResponseMessage> AddEvent([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/addCommunication")]
        Task<HttpResponseMessage> AddCommunication([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);
        [Post("/sandbox/mobilev1.asmx/addStakeholder")]
        Task<HttpResponseMessage> AddStakeholder([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getFormAndDropDownFieldValues")]
        Task<HttpResponseMessage> GetFormAndDropDownFieldValues([Body] FormFieldBody formFieldBody, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getEventDetails")]
        Task<HttpResponseMessage> GetEventDetails([Body] APIRequestExtraBody aPIRequestExtraBody, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getAllCommunications")]
        Task<HttpResponseMessage> GetAllCommunications([Body] APIRequestBody aPIRequestBody, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getGroupStakeholderDetails")]
        Task<HttpResponseMessage> GetGroupStakeholderDetails([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getIndividualStakeholderDetails")]
        Task<HttpResponseMessage> GetIndividualStakeholderDetails([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getLandParcelStakeholderDetails")]
        Task<HttpResponseMessage> GetLandParcelStakeholderDetails([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/getAllStakeholders")]
        Task<HttpResponseMessage> GetAllStakeholders([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/getCommunicationDetails")]
        Task<HttpResponseMessage> GetCommunicationDetails([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/changePassword")]
        Task<HttpResponseMessage> ChangePassword([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getProjectTeam")]
        Task<HttpResponseMessage> GetProjectTeam([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getIssues")]
        Task<HttpResponseMessage> GetIssues([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getIssueDetails")]
        Task<HttpResponseMessage> GetIssueDetails([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/getProjectTeamMemberDetails")]
        Task<HttpResponseMessage> GetProjectTeamMemberDetails([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/DelRec")]
        Task<HttpResponseMessage> DelRec([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/RequestUsr")]
        Task<HttpResponseMessage> RequestUsr([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/RequestPwd")]
        Task<HttpResponseMessage> RequestPwd([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);


        [Post("/sandbox/mobilev1.asmx/getProjectList")]
        Task<HttpResponseMessage> GetProjectList([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

        [Post("/sandbox/mobilev1.asmx/switchProject")]
        Task<HttpResponseMessage> SwitchProject([Body] jsonTextObj jsonTextObj, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);




    }
}
