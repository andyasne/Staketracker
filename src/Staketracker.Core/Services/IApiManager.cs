using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Staketracker.Core.Services
{
    public interface IApiManager
    {
       Task<HttpResponseMessage> GetMakeUps(string brand);
       Task<HttpResponseMessage> GetNews();
    }
}
