
using System;

namespace Subway.Status.Domain.Dtos
{
    public class ServiceAlert
    {
        public Alert[] Alerts { get; set; }
    }

    public class Alert : IIdentificable
    {
        public string Id { get; set; }
        public string RouteId { get; set; }
        public string StopId { get; set; }
        public string HeaderText { get; set; }
        public string DescriptionText { get; set; }
        public int Cause { get; set; }
        public int Effect { get; set; }
        public DateTime ReportedDate { get; set; }
    }
}
