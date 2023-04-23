using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sometimes.Database.Models
{
    public class UserMessages
    {
        [BsonId]
        public string UUID { get; set; } = null!;
        public List<Message> Messages { get; set; } = null!;
    }

    public class Message
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageID { get; set; } = null!;
        public DateTime? SentTime { get; set; }
        public string Body { get; set; } = null!;
        public string? SenderUUID { get; set; }
        public DateTime? ReadTime { get; set; }
        public bool Read { get; set; } = false;
    }
}
