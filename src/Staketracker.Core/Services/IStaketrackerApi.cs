using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;

namespace Staketracker.Core
{
    [Headers("Content-Type: application/json")]
    public interface IStaketrackerApi
    {
        [Post("/demo/mobilev1.asmx/authenticateUser")]
        Task<HttpResponseMessage> AuthenticateUser([Body] LoginAPIBody loginApiBody, CancellationToken cancellationToken);


        [Post("/demo/mobilev1.asmx/Is2FEnabled")]
        Task<HttpResponseMessage> Is2FEnabled([Body] LoginAPIBody loginApiBody, CancellationToken cancellationToken);


        [Post("/demo/mobilev1.asmx/getUsrEmail")]
        Task<HttpResponseMessage> GetUsrEmail([Body] LoginAPIBody loginApiBody, CancellationToken cancellationToken);

        [Post("/demo/mobilev1.asmx/getEvents")]
        Task<HttpResponseMessage> GetEvents([Body] APIRequestBody aPIRequestBody, [Header("sessionId")] string sessionId, CancellationToken cancellationToken);

    }
}
