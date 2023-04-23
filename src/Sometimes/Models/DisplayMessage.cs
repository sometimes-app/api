using MongoDB.Bson.Serialization.Attributes;

namespace Sometimes.Models
{
    public class DisplayMessage
    {
        public string uuid { get; set; } = null!;
        public string messageId { get; set; } = null!;
        public DateTime sentTime { get; set; }
        public string messageBody { get; set; } = null!;
        public string senderName { get; set; } = null!;
        public string senderUuid { get; set; } = null!;
    }
}
