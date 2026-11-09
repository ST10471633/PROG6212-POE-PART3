
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public string CategoryType { get; set; } = string.Empty;

        public int EventID { get; set; }
    }
}
