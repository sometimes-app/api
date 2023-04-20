using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoCollection<UserInfo> UserInfoCollection;
        private readonly IMongoCollection<UserMessages> UserMessagesCollection;
        private readonly IMongoCollection<PremadeMessage> PremadeMessagesCollection;

        public DatabaseService(IOptions<SometimesDbInfo> sometimesDbInfo)
        {
            var mongoClient = new MongoClient(AppSettingsConfig.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(sometimesDbInfo.Value.DatabaseName);

            UserInfoCollection = mongoDatabase.GetCollection<UserInfo>(sometimesDbInfo.Value.UserInfoCollectionName);
            UserMessagesCollection = mongoDatabase.GetCollection<UserMessages>(sometimesDbInfo.Value.MessagesCollectionInfo);
            PremadeMessagesCollection = mongoDatabase.GetCollection<PremadeMessage>(sometimesDbInfo.Value.PremadeCollectionInfo);
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

        public async Task<Message?> GetDailyMessage(string uuid)
        {
            UserMessages userMessages = await UserMessagesCollection.Find(x => x.UUID == uuid).FirstOrDefaultAsync();
            // user not found
            if (userMessages == null)
                return null;

            var unreadMessages = userMessages.Messages.Where(m => m.Read == false);
            // there are unread messages
            if (unreadMessages.Count() > 0)
            {
                var random = new Random();
                var index = random.Next(0, unreadMessages.Count());
                return unreadMessages.ElementAt(index);
            }

            // the user has no unread messeges, get a premade message and add to users messages list and return message
            PremadeMessage result = await PremadeMessagesCollection.AsQueryable().Sample(1).FirstOrDefaultAsync();
            var newMessage = new Message { Body = result.Body, MessageID = result.MessageID, SentTime = DateTime.Now };
            var filter = Builders<UserMessages>
             .Filter.Eq(user => user.UUID, uuid);

            var update = Builders<UserMessages>.Update
                    .Push(user => user.Messages, newMessage);

            await UserMessagesCollection.FindOneAndUpdateAsync(filter, update);
            return newMessage;
        }

        public async Task<bool> ReadMessage(string messageID)
        {
            var filter = Builders<UserMessages>.Filter.ElemMatch(u => u.Messages, m => m.MessageID == messageID);
            var update = Builders<UserMessages>.Update.Set("Messages.$.Read", true);
            var result = await UserMessagesCollection.FindOneAndUpdateAsync(filter, update);
            return result is not null ? true : false;
        }

    //    private void Temp()
    //    {
    //        var pResults = PremadeMessagesCollection.Aggregate()
    //.Match(new BsonDocument { { "username", "nraboy" } })
    //.Project(new BsonDocument{
    //        { "_id", 1 },
    //        { "username", 1 },
    //        {
    //            "items", new BsonDocument{
    //                {
    //                    "$map", new BsonDocument{
    //                        { "input", "$items" },
    //                        { "as", "item" },
    //                        {
    //                            "in", new BsonDocument{
    //                                {
    //                                    "$convert", new BsonDocument{
    //                                        { "input", "$$item" },
    //                                        { "to", "objectId" }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    })
    //.Lookup("movies", "items", "_id", "movies")
    //.Unwind("movies")
    //.Group(new BsonDocument{
    //        { "_id", "$_id" },
    //        {
    //            "username", new BsonDocument{
    //                { "$first", "$username" }
    //            }
    //        },
    //        {
    //            "movies", new BsonDocument{
    //                { "$addToSet", "$movies" }
    //            }
    //        }
    //    })
    //.ToList();

    //        foreach (var pResult in pResults)
    //        {
    //            Console.WriteLine(pResult);
    //        }
    //    }
    }
}

