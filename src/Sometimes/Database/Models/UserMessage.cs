using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sometimes.Database.Models
{
	public class UserMessage
	{
        [BsonId]
        public string userUUID { get; set; } = null!;

        public List<Message> Messages { get; set; } = null!;
    }

    public class Message
    {
        public string friendUUID { get; set; } = null!;
        public string createTime { get; set; } = null!;
        public string? readTime { get; set; }
        public string messageBody { get; set; } = null!;
    }
}

