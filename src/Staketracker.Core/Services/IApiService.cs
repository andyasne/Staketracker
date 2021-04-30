using Fusillade;

namespace Staketracker.Core.Services
{
    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
