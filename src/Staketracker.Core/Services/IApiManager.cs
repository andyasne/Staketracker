using System.Net.Http;
using System.Threading.Tasks;
using Staketracker.Core.Models;

namespace Staketracker.Core.Services
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> AuthenticateUser(LoginAPIBody loginApiBody);

        Task<HttpResponseMessage> Is2FEnabled(LoginAPIBody loginApiBody);

        Task<HttpResponseMessage> GetUsrEmail(LoginAPIBody loginApiBody);



    }
}
