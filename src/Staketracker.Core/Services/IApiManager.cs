using System.Net.Http;
using System.Threading.Tasks;
using Staketracker.Core.Models;

namespace Staketracker.Core.Services
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> AuthenticateUser(LoginAPIBody loginApiBody);
    }
}
