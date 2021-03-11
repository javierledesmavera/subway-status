using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Subway.Status.Business.Contracts;
using Subway.Status.Domain.Dtos;

namespace Subway.Status.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubwayController : ControllerBase
    {
        private readonly ILogger<SubwayController> _logger;
        private readonly ISubwayBusiness _business;

        public SubwayController(ILogger<SubwayController> logger, ISubwayBusiness business)
        {
            _logger = logger;
            _business = business;
        }

        [HttpGet("/serviceAlerts")]
        public async Task<ServiceAlerts> GetServiceAlerts()
        {
            return await this._business.GetServiceAlerts();
        }
    }
}