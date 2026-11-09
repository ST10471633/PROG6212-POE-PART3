
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class Event
    {
        public int EventID { get; set; }

        [Required]
        public string EventName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public decimal Distance { get; set; }

        public int EventTypeID { get; set; }

        public int OrganiserID { get; set; }
    }
}
