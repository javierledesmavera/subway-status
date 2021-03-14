using Microsoft.AspNetCore.Mvc;
using Subway.Status.Api.Models.Request;
using Subway.Status.Business.Contracts;
using Subway.Status.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Subway.Status.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubwayController : ControllerBase
    {
        private readonly ISubwayBusiness _business;

        public SubwayController(ISubwayBusiness business)
        {
            _business = business;
        }

        [HttpGet("/serviceAlerts")]
        [Produces(typeof(ServiceAlert))]
        public async Task<IActionResult> GetServiceAlerts()
        {
            return Ok(await _business.GetServiceAlerts());
        }

        [HttpGet("/lines")]
        [Produces(typeof(IEnumerable<Line>))]
        public async Task<IActionResult> GetSubwayLines()
        {
            return Ok(await _business.GetSubwayLines());
        }

        [HttpGet("/stops/{lineId}")]
        [Produces(typeof(IEnumerable<Stop>))]
        public async Task<IActionResult> GetStopsByLineId(string lineId)
        {
            return Ok(await _business.GetStopsByLineId(lineId));
        }


        [HttpGet("/stops/{lineId}/headers/{stopId}")]
        [Produces(typeof(IEnumerable<Stop>))]
        public async Task<IActionResult> GetStopHeadersByLineIdAndStopId(string lineId, string stopId)
        {
            return Ok(await _business.GetStopHeadersByLineIdAndStopId(lineId, stopId));
        }

        [HttpGet("/arrivals/{lineId}/stops/{stopId}")]
        [Produces(typeof(DateTime))]
        public async Task<IActionResult> GetNextArrivalToStop(string lineId, string stopId, [FromQuery][Required]string destinationStopId)
        {
            return Ok(await _business.GetNextArrivalToStop(lineId, stopId, destinationStopId));
        }

        [HttpPost("/alerts/{lineId}")]
        [Produces(typeof(IEnumerable<Alert>))]
        public async Task<IActionResult> GetAlertsFiltered(string lineId, [FromBody]GetAlertsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return Ok(await _business.GetAlertsFiltered(lineId, request.FromDate.Value, request.ToDate.Value));
        }
    }
}