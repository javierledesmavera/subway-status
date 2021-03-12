using Subway.Status.Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subway.Status.Business.Contracts
{
    public interface ISubwayBusiness
    {
        Task<ServiceAlert> GetServiceAlerts();

        Task<IEnumerable<Domain.Dtos.Line>> GetSubwayLines();

        Task<ServiceAlert> GetStopsByLineId(string lineId);
    }
}
