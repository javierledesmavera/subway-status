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

        /// <summary>
        /// Obtiene las alertas actuales del servicio y persiste en BD las que no existan para el día de hoy
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Obtiene todas las lineas de subte
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Obtiene las estaciones para una linea de subte
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Obtiene las estaciones cabeceras para una linea de subte y excluye la estacion de origen para que no se pueda seleccionar a si misma como destino
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="stopId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="stopId"></param>
        /// <param name="destinationStopId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Obtiene las alertas filtrando por linea de subte y un rango de fechas
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Guarda las alertas en la base de datos
        /// </summary>
        /// <param name="serviceAlertResponse"></param>
        /// <returns></returns>
        private async Task PersistServiceAlerts(Domain.Dtos.ServiceAlert serviceAlertResponse)
        {
            if (serviceAlertResponse != null && serviceAlertResponse.Alerts != null && serviceAlertResponse.Alerts.Any())
            {
                foreach (var serviceAlert in serviceAlertResponse.Alerts)
                {
                    // Se trae las alertas para la linea que se esta iterando, con el mismo texto en el día de hoy
                    var filteredAlerts = this._alertRepository.GetFiltered(alert => alert.RouteId == serviceAlert.RouteId &&
                        alert.DescriptionText == serviceAlert.DescriptionText &&
                        alert.AlertDate.Date == DateTime.Now.Date);

                    // Si no hay alertas que cumplan esas condiciones, la misma es una nueva alerta y se inserta
                    if (!filteredAlerts.Any())
                    {
                        Repository.Entities.Alert entity = _mapper.Map<Repository.Entities.Alert>(serviceAlert);
                        entity.AlertDate = DateTime.Now;
                        await _alertRepository.AddAsync(entity);
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el id de direccion usado en la API según si la estacion de origen es una estacion anterior a la de destino
        /// </summary>
        /// <param name="stopId"></param>
        /// <param name="destinationStopId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Obtiene el id de la estacion sin las letas N o S para que sólo sea el id numérico
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        private string GetStopIdWithoutDirection(string stopId)
        {
            string normalizedStopId = stopId.Replace("N", string.Empty);
            normalizedStopId = normalizedStopId.Replace("S", string.Empty);
            return normalizedStopId;
        }

        /// <summary>
        /// Convierte la unidad de tiempo devuelta por la api a DateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        private DateTime GetDateTimeFromUnixTime(int unixTime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
            return dtDateTime;
        }
    }
}
