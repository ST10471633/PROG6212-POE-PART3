
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int ParticipantID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
