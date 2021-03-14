using System;
using System.ComponentModel.DataAnnotations;

namespace Subway.Status.Api.Models.Request
{
    public class GetAlertsRequest
    {
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
    }
}
