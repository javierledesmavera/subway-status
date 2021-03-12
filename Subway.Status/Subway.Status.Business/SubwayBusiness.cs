using AutoMapper;
using Microsoft.Extensions.Configuration;
using Subway.Status.Business.Contracts;
using Subway.Status.Domain;
using Subway.Status.Integration.Contracts;
using Subway.Status.Integration.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subway.Status.Business
{
    public class SubwayBusiness : ISubwayBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly ISubwaysApi _subwayApi;
        private readonly IMapper _mapper;

        public SubwayBusiness(IConfiguration configuration, ISubwaysApi subwayApi, IMapper mapper)
        {
            _configuration = configuration;
            _clientId = this._configuration.GetSection(Constants.SubwayApiClientId).Value;
            _clientSecret = this._configuration.GetSection(Constants.SubwayApiClientSecret).Value;
            _subwayApi = subwayApi;
            _mapper = mapper;
        }

        public async Task<Domain.Dtos.ServiceAlert> GetServiceAlerts()
        {
            try
            {
                SubwayApiResponse<ServiceAlertsHeader, Integration.Entities.ServiceAlerts> response = await this._subwayApi.GetServiceAlerts(_clientId, _clientSecret);
                return _mapper.Map<Domain.Dtos.ServiceAlert>(response);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Domain.Dtos.Line>> GetSubwayLines()
        {
            try
            {
                SubwayApiResponse<ForecastGtfsHeader, Integration.Entities.ForecastGtfs> response = await this._subwayApi.GetForecastGtfs(_clientId, _clientSecret);
                return response.Entity.Select(entity => entity.Line.RouteId).Distinct().Select(routeId => new Domain.Dtos.Line { Id = routeId });
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<Domain.Dtos.ServiceAlert> GetStopsByLineId(string lineId)
        {
            try
            {
                SubwayApiResponse<ForecastGtfsHeader, Integration.Entities.ForecastGtfs> response = await this._subwayApi.GetForecastGtfs(_clientId, _clientSecret);
                return _mapper.Map<Domain.Dtos.ServiceAlert>(response);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
