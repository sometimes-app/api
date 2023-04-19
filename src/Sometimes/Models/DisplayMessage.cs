using MongoDB.Bson.Serialization.Attributes;

namespace Sometimes.Models
{
    public class DisplayMessage
    {
        [BsonId]
        public string MessageID { get; set; } = null!;
        public string MessageBody { get; set; } = null!;
        public string SenderName { get; set; } = null!;
    }
}
