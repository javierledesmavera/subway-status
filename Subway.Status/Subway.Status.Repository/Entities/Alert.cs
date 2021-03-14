using System;
using System.ComponentModel.DataAnnotations;

namespace Subway.Status.Repository.Entities
{
    public class Alert
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RouteId { get; set; }
        public string StopId { get; set; }
        [Required]
        public string HeaderText { get; set; }
        [Required]
        public string DescriptionText { get; set; }
        public int Cause { get; set; }
        public int Effect { get; set; }
        public DateTime AlertDate { get; set; }
    }
}
