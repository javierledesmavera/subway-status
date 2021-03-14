using Newtonsoft.Json;

namespace Subway.Status.Integration.Entities
{
    public class ForecastGtfs : SubwayBaseEntity
    {
        [JsonProperty("Linea")]
        public Line Line { get; set; }
    }

    public class Line
    {
        [JsonProperty("Trip_Id")]
        public string TripId { get; set; }
        [JsonProperty("Route_Id")]
        public string RouteId { get; set; }
        [JsonProperty("Direction_ID")]
        public int DirectionId { get; set; }
        [JsonProperty("start_time")]
        public string StartTime { get; set; }
        [JsonProperty("start_date")]
        public string StartDate { get; set; }
        [JsonProperty("Estaciones")]
        public Stop[] Stops { get; set; }
    }

    public class Stop
    {
        [JsonProperty("stop_id")]
        public string StopId { get; set; }
        [JsonProperty("stop_name")]
        public string StopName { get; set; }
        public StopTimeInfo Arrival { get; set; }
        public StopTimeInfo Departure { get; set; }
    }

    public class StopTimeInfo
    {
        public int Time { get; set; }
        public int Delay { get; set; }
    }
}
