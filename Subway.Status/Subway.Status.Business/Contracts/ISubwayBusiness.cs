using Subway.Status.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subway.Status.Business.Contracts
{
    public interface ISubwayBusiness
    {
        Task<ServiceAlert> GetServiceAlerts();

        Task<IEnumerable<Domain.Dtos.Line>> GetSubwayLines();

        Task<IEnumerable<Domain.Dtos.Stop>> GetStopsByLineId(string lineId);

        Task<IEnumerable<Domain.Dtos.Stop>> GetStopHeadersByLineIdAndStopId(string lineId, string stopId);

        Task<System.DateTime> GetNextArrivalToStop(string lineId, string stopId, string destinationStopId);

        IEnumerable<Domain.Dtos.Alert> GetAlertsFiltered(string lineId, DateTime fromDate, DateTime toDate);
    }
}
