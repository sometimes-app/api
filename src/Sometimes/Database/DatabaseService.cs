using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoCollection<UserInfo> UserInfoCollection;
        private readonly IMongoCollection<UserMessage> UserMessagesCollection;

        public DatabaseService(IOptions<SometimesDbInfo> sometimesDbInfo)
        {
            var mongoClient = new MongoClient(AppSettingsConfig.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(sometimesDbInfo.Value.DatabaseName);

            UserInfoCollection = mongoDatabase.GetCollection<UserInfo>(sometimesDbInfo.Value.UserInfoCollectionName);
            UserMessagesCollection = mongoDatabase.GetCollection<UserMessage>(sometimesDbInfo.Value.MessagesCollectionInfo);
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


        public async Task<List<UserMessage>> GetUserMessagesAsync() =>
            await UserMessagesCollection.Find(_ => true).ToListAsync();

        public async Task<UserMessage?> GetUserMessagesAsync(string id) =>
            await UserMessagesCollection.Find(x => x.userUUID == id).FirstOrDefaultAsync();

        public async Task CreateUserMessageAsync(UserMessage newMessage) =>
            await UserMessagesCollection.InsertOneAsync(newMessage);

        public async Task UpdateUserMessageAsync(string id, UserMessage userMessage) =>
            await UserMessagesCollection.ReplaceOneAsync(x => x.userUUID == id, userMessage);

        public async Task RemoveUserMessageAsync(string id) =>
            await UserMessagesCollection.DeleteOneAsync(x => x.userUUID == id);
    }
}

