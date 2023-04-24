using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sometimes.Database.Models
{
	public class UserInfo
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = null!;

        public string uuid { get; set; } = null!;

		public string FirstName { get; set; } = null!;

		public string LastName { get; set; } = null!;

		public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string ProfilePicUrl { get; set; } = null!;

        public List<string> Friends { get; set; } = null!;
    }
}

