using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Subway.Status.Business.Contracts;
using Subway.Status.Domain;
using Subway.Status.Domain.Dtos;
using Subway.Status.Integration.Contracts;
using Subway.Status.Integration.Entities;
using Subway.Status.Repository;
using Subway.Status.Repository.Contracts;
using Subway.Status.Repository.Entities;
using System;
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
        private readonly IRepository<Repository.Entities.Alert> _alertRepository;

        public SubwayBusiness(IConfiguration configuration, ISubwaysApi subwayApi, IMapper mapper, IRepository<Repository.Entities.Alert> alertRepository)
        {
            _configuration = configuration;
            _clientId = this._configuration.GetSection(Constants.SubwayApiClientId).Value;
            _clientSecret = this._configuration.GetSection(Constants.SubwayApiClientSecret).Value;
            _subwayApi = subwayApi;
            _mapper = mapper;
            _alertRepository = alertRepository;
        }

        public async Task<Domain.Dtos.ServiceAlert> GetServiceAlerts()
        {
            try
            {
                SubwayApiResponse<ServiceAlertsHeader, Integration.Entities.ServiceAlert> response = await this._subwayApi.GetServiceAlerts(_clientId, _clientSecret);
                Domain.Dtos.ServiceAlert serviceAlertResponse = _mapper.Map<Domain.Dtos.ServiceAlert>(response);
                await PersistServiceAlerts(serviceAlertResponse);
                return serviceAlertResponse;
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

        public async Task<IEnumerable<Domain.Dtos.Stop>> GetStopsByLineId(string lineId)
        {
            try
            {
                SubwayApiResponse<ForecastGtfsHeader, Integration.Entities.ForecastGtfs> response = await this._subwayApi.GetForecastGtfs(_clientId, _clientSecret);
                var route = response.Entity.Where(entity => entity.Line.RouteId == lineId).FirstOrDefault();
                if (route != null)
                {
                    return route.Line.Stops.Select(stop => new Domain.Dtos.Stop { Id = stop.StopId, Description = stop.StopName });
                }
                else
                {
                    throw new Exception("La linea que se desea consultar no existe.");
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Domain.Dtos.Stop>> GetStopHeadersByLineIdAndStopId(string lineId, string stopId)
        {
            try
            {
                var stops = await this.GetStopsByLineId(lineId);
                var stopHeaders = new List<Domain.Dtos.Stop> { stops.First(), stops.Last() };
                return stopHeaders.Where(stop => stop.Id != stopId);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<System.DateTime> GetNextArrivalToStop(string lineId, string stopId, string destinationStopId)
        {
            try
            {
                SubwayApiResponse<ForecastGtfsHeader, Integration.Entities.ForecastGtfs> response = await this._subwayApi.GetForecastGtfs(_clientId, _clientSecret);
                var routes = response.Entity.Where(entity => entity.Line.RouteId == lineId);

                if (routes != null && routes.Any())
                {
                    var route = routes.Where(route => route.Line.DirectionId == GetDirectionByStops(stopId, destinationStopId)).FirstOrDefault();

                    if (route != null)
                    {
                        if (route.Line.Stops.Select(stop => GetStopIdWithoutDirection(stop.StopId)).Contains(GetStopIdWithoutDirection(stopId)) &&
                            route.Line.Stops.Select(stop => GetStopIdWithoutDirection(stop.StopId)).Contains(GetStopIdWithoutDirection(destinationStopId)))
                        {
                            return route.Line.Stops.Where(stop =>
                            GetStopIdWithoutDirection(stop.StopId) == GetStopIdWithoutDirection(stopId))
                            .Select(stop => GetDateTimeFromUnixTime(stop.Arrival.Time)).FirstOrDefault();
                        }
                        else
                        {
                            throw new Exception("Las estaciones elegidas no pertenecen a la linea seleccionada.");
                        }
                    }
                    else
                    {
                        throw new Exception("Las estaciones seleccionadas no existen.");
                    }
                }
                else
                {
                    throw new Exception("La linea que se desea consultar no existe.");
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Domain.Dtos.Alert> GetAlertsFiltered(string lineId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                IEnumerable<Repository.Entities.Alert> alerts = _alertRepository.GetFiltered(alert => alert.RouteId == lineId && fromDate.Date <= alert.AlertDate.Date && toDate.Date >= alert.AlertDate.Date);
                return _mapper.Map<IEnumerable<Domain.Dtos.Alert>>(alerts);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task PersistServiceAlerts(Domain.Dtos.ServiceAlert serviceAlertResponse)
        {
            if (serviceAlertResponse != null && serviceAlertResponse.Alerts != null && serviceAlertResponse.Alerts.Any())
            {
                foreach (var serviceAlert in serviceAlertResponse.Alerts)
                {
                    var filteredAlerts = this._alertRepository.GetFiltered(alert => alert.RouteId == serviceAlert.RouteId &&
                        alert.DescriptionText == serviceAlert.DescriptionText &&
                        alert.AlertDate.Date == DateTime.Now.Date);

                    if (!filteredAlerts.Any())
                    {
                        Repository.Entities.Alert entity = _mapper.Map<Repository.Entities.Alert>(serviceAlert);
                        entity.AlertDate = DateTime.Now;
                        await _alertRepository.AddAsync(entity);
                    }
                }
            }
        }

        private int GetDirectionByStops(string stopId, string destinationStopId)
        {
            if (Convert.ToInt32(GetStopIdWithoutDirection(stopId)) < Convert.ToInt32(GetStopIdWithoutDirection(destinationStopId)))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private string GetStopIdWithoutDirection(string stopId)
        {
            string normalizedStopId = stopId.Replace("N", string.Empty);
            normalizedStopId = normalizedStopId.Replace("S", string.Empty);
            return normalizedStopId;
        }

        private DateTime GetDateTimeFromUnixTime(int unixTime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
            return dtDateTime;
        }
    }
}
