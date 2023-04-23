using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog;
using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoCollection<UserInfo> UserInfoCollection;
        private readonly string UserInfoCollectionName;
        private readonly IMongoCollection<UserMessages> UserMessagesCollection;
        private readonly string UserMessagesCollectionName;
        private readonly IMongoCollection<PremadeMessage> PremadeMessagesCollection;
        private readonly string PremadeMessagesCollectionName;

        public DatabaseService(IOptions<SometimesDbInfo> sometimesDbInfo)
        {
            var mongoClient = new MongoClient(AppSettingsConfig.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(sometimesDbInfo.Value.DatabaseName);

            UserInfoCollectionName = sometimesDbInfo.Value.UserInfoCollectionName.ValidatedNullable()!;
            UserMessagesCollectionName = sometimesDbInfo.Value.MessagesCollectionInfo.ValidatedNullable()!;
            PremadeMessagesCollectionName = sometimesDbInfo.Value.PremadeCollectionInfo.ValidatedNullable()!;

            UserInfoCollection = mongoDatabase.GetCollection<UserInfo>(UserInfoCollectionName);
            UserMessagesCollection = mongoDatabase.GetCollection<UserMessages>(UserMessagesCollectionName);
            PremadeMessagesCollection = mongoDatabase.GetCollection<PremadeMessage>(PremadeMessagesCollectionName);
        }


        public async Task<List<UserInfo>> GetAllUserInfoAsync() =>
            await UserInfoCollection.Find(_ => true).ToListAsync();

        public async Task<UserInfo?> GetUserInfoAsync(string id) =>
            await UserInfoCollection.Find(x => x.UUID == id).FirstOrDefaultAsync();

        public async Task CreateUserInfoAsync(UserInfo newUser) =>
            await UserInfoCollection.InsertOneAsync(newUser);

        public async Task UpdateUserInfoAsync(string id, UserInfo userInfo) =>
            await UserInfoCollection.ReplaceOneAsync(x => x.UUID == id, userInfo);

        public async Task RemoveUserInfoAsync(string id) =>
            await UserInfoCollection.DeleteOneAsync(x => x.UUID == id);

        /// <inheritdoc/>
        public async Task<List<UserInfo>?> GetFriends(string uuid)
        {
            var userInfo = await GetUserInfoAsync(uuid);
            if(userInfo == null)
            {
                return null;
            }

            FilterDefinitionBuilder<UserInfo> friendListFilterBuilder = new();
            var friendListFilter = friendListFilterBuilder.In(user => user.UUID, userInfo.Friends);
            return await UserInfoCollection.Find(friendListFilter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> AddFriend(string userUuid, string friendUuid)
        {
            var addFriend = Builders<UserInfo>.Update.AddToSet(user => user.Friends, friendUuid);
            var result = await UserInfoCollection.UpdateOneAsync(user => user.UUID == userUuid, addFriend);
            return result.IsAcknowledged;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFriend(string userUuid, string friendUuid)
        {
            var removeFriend = Builders<UserInfo>.Update.Pull(user => user.Friends, friendUuid);
            var result = await UserInfoCollection.UpdateOneAsync(user => user.UUID == userUuid, removeFriend);
            return result.IsAcknowledged;
        }

        public async Task<DisplayMessage?> GetDailyMessage(string uuid)
        {
            //var unreadMessages = await UserMessagesCollection.AsQueryable()
            //    .SelectMany(f => f.messages)
            //    .Where(m => !m.read)
            //    .ToListAsync();



            var unreadMessages2 = await UserMessagesCollection.Aggregate()
                .Match(user => user.uuid == uuid)
                .Unwind(u => u.messages)
                .Match(m => !m["messages"]["read"].AsBoolean)
                .Lookup<UserInfo, BsonDocument>(
                    UserInfoCollectionName,
                    "messages.$.senderUuid",
                    "uuid",
                    "senderInfo"
                )
                .ToListAsync();

            //var unreadMessages = await UserMessagesCollection.Aggregate()
            //    .Match(user => user.uuid == uuid)
            //    .Unwind(u => u.messages)
            //    .Match(m => !m["messages"]["read"].AsBoolean)
            //    .Lookup(
            //        UserInfoCollectionName,
            //        u => u["uuid"],
            //        m => m["uuid"],
            //        (messages, users) => new BsonDocument
            //        {
            //            { "uuid": users["uuid"] }
            //        }


            //    )
            //    .Project(p => new
            //    {
            //        UserId = p["UserId"],
            //        Username = p["Username"],
            //        Messages = p["Messages"]
            //            .AsBsonArray
            //            .Select(x => new BsonDocument("read", x["read"])
            //                .Add("text", x["text"])
            //                .Add("timestamp", x["timestamp"])
            //            )
            //            .ToList()
            //    })
            //    .ToListAsync();

            if (unreadMessages2.Count > 0)
            {
                var message = unreadMessages2.ElementAt(new Random().Next(0, unreadMessages2.Count));
                return new DisplayMessage();
                //{
                //    uuid = message
                //    messageId
                //    sentTime
                //    messageBody
                //    senderName
                //    senderUuid
                //}

            }
            else
            {
                return null;
            }

            #region temp
            //UserMessages userMessages = await UserMessagesCollection.Find(x => x.uuid == uuid).FirstOrDefaultAsync();
            //// user not found
            //if (userMessages == null)
            //    return null;

            //var unreadMessages = userMessages.messages.Where(m => m.read == false);
            //// there are unread messages
            //if (unreadMessages.Count() > 0)
            //{
            //    var random = new Random();
            //    var index = random.Next(0, unreadMessages.Count());
            //    return unreadMessages.ElementAt(index);
            //}

            //// the user has no unread messeges, get a premade message and add to users messages list and return message
            //PremadeMessage result = await PremadeMessagesCollection.AsQueryable().Sample(1).FirstOrDefaultAsync();
            //var newMessage = new DisplayMessage { body = result.Body, messageId = result.MessageID, sentTime = DateTime.Now };
            //var filter = Builders<UserMessages>
            // .Filter.Eq(user => user.uuid, uuid);

            //var update = Builders<UserMessages>.Update
            //        .Push(user => user.messages, newMessage);

            //await UserMessagesCollection.FindOneAndUpdateAsync(filter, update);
            //return newMessage;
            #endregion
        }

        /// <inheritdoc/>
        public async Task<bool> ReadMessage(string uuid, string messageID)
        {
            var findMessageFilter = Builders<UserMessages>.Filter.And(
                Builders<UserMessages>.Filter.Eq("uuid", uuid),
                Builders<UserMessages>.Filter.Eq("messages.messageId", messageID)
            );

            var updateRead = Builders<UserMessages>.Update.Set("messages.$.read", true)
                .Set("messages.$.readTime", DateTime.UtcNow);
            var value = await UserMessagesCollection.UpdateOneAsync(findMessageFilter, updateRead);

            return value is not null ? true : false;
        }
    }
}

