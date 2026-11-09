
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class Result
    {
        public int ResultsID { get; set; }

        public int EnrollmentID { get; set; }

        public string? FinishTime { get; set; }

        public int? FinishPosition { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
