using Newtonsoft.Json;

namespace Subway.Status.Integration.Entities
{
    public class ServiceAlerts
    {
        public Header Header { get; set; }
        public Entity[] Entity { get; set; }
    }

    public class Header
    {
        [JsonProperty("gtfs_realtime_version")]
        public string GtfsRealtimeVersion { get; set; }
        public int Incrementality { get; set; }
        public int Timestamp { get; set; }
    }

    public class Entity
    {
        public string Id { get; set; }
        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("Trip_update")]
        public object TripUpdate { get; set; }
        public object Vehicle { get; set; }
        public Alert Alert { get; set; }
    }

    public class Alert
    {
        [JsonProperty("active_period")]
        public object[] ActivePeriod { get; set; }
        [JsonProperty("informed_entity")]
        public InformedEntity[] InformedEntity { get; set; }
        public int Cause { get; set; }
        public int Effect { get; set; }
        public object Url { get; set; }
        [JsonProperty("header_text")]
        public HeaderText HeaderText { get; set; }
        [JsonProperty("description_text")]
        public DescriptionText DescriptionText { get; set; }
    }

    public class HeaderText
    {
        public Translation[] Translation { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public string Language { get; set; }
    }

    public class DescriptionText
    {
        public Translation[] Translation { get; set; }
    }

    public class InformedEntity
    {
        [JsonProperty("agency_id")]
        public string AgencyId { get; set; }
        [JsonProperty("route_id")]
        public string RouteId { get; set; }
        [JsonProperty("route_type")]
        public int RouteType { get; set; }
        public object trip { get; set; }
        [JsonProperty("stop_id")]
        public string StopId { get; set; }
    }
}
