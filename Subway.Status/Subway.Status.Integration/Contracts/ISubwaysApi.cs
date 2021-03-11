using Refit;
using Subway.Status.Integration.Entities;
using System.Threading.Tasks;

namespace Subway.Status.Integration.Contracts
{
    public interface ISubwaysApi
    {
        [Get("/subtes/serviceAlerts")]
        Task <ServiceAlerts> GetServiceAlerts([AliasAs("client_id")]string clientId, [AliasAs("client_secret")]string clientSecret, int json = 1);
    }
}
