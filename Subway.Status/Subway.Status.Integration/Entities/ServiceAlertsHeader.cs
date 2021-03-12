using Newtonsoft.Json;

namespace Subway.Status.Integration.Entities
{
    public class ServiceAlertsHeader : SubwayResponseBaseHeader
    {
        [JsonProperty("gtfs_realtime_version")]
        public string GtfsRealtimeVersion { get; set; }
        public int Incrementality { get; set; }
    }
}
