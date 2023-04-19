using MongoDB.Bson.Serialization.Attributes;

namespace Sometimes.Database.Models
{
    public class PremadeMessage
    {
        [BsonId]
        public string MessageID { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
