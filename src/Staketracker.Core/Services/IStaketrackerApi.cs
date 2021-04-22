using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Staketracker.Core.Models;

namespace Staketracker.Core
{
    [Headers("Content-Type: application/json")]
    public interface IStaketrackerApi
    {
        [Post("/uat/mobilev1.asmx/authenticateUser")]
        Task<HttpResponseMessage> AuthenticateUser([Body] LoginAPIBody loginApiBody, CancellationToken cancellationToken);
    }
}
