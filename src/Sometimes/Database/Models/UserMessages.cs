using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sometimes.Database.Models
{
    public class UserMessages
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = null!;
        public string uuid { get; set; } = null!;
        public List<UserMessage> messages { get; set; } = new List<UserMessage>();
    }

    public class UserMessage
    {
        public string _id { get; set; } = null!;
        public string messageId { get; set; } = null!;
        public DateTime sentTime { get; set; }
        public string body { get; set; } = null!;
        public string? senderUuid { get; set; }
        public DateTime? readTime { get; set; }
        public bool read { get; set; } = false;
    }
}
