using System.Text.Json.Serialization;

namespace RaceDayMVC.Models
{
    public class EventType
    {
        public int EventTypeID { get; set; }

        [JsonPropertyName("typeName")]
        public string EventTypeName { get; set; } = string.Empty;
    }
}