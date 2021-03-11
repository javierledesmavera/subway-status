using AutoMapper;
using Microsoft.Extensions.Configuration;
using Subway.Status.Business.Contracts;
using Subway.Status.Domain;
using Subway.Status.Domain.Dtos;
using Subway.Status.Integration.Contracts;
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

        public async Task<ServiceAlerts> GetServiceAlerts()
        {
            try
            {
                var response = await this._subwayApi.GetServiceAlerts(_clientId, _clientSecret);
                return _mapper.Map<ServiceAlerts>(response);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
