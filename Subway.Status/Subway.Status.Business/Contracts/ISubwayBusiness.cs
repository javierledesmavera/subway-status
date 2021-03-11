using Subway.Status.Domain.Dtos;
using System.Threading.Tasks;

namespace Subway.Status.Business.Contracts
{
    public interface ISubwayBusiness
    {
        Task<ServiceAlerts> GetServiceAlerts();
    }
}
