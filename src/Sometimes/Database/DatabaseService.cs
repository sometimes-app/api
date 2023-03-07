using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoCollection<UserInfo> UserInfo;
        private readonly IMongoCollection<UserMessage> UserMessages;

        public DatabaseService(IOptions<SometimesDbInfo> sometimesDbInfo)
        {
            var mongoClient = new MongoClient(AppSettingsConfig.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(sometimesDbInfo.Value.DatabaseName);

            UserInfo = mongoDatabase.GetCollection<UserInfo>(sometimesDbInfo.Value.UserInfoCollectionName);
            UserMessages = mongoDatabase.GetCollection<UserMessage>(sometimesDbInfo.Value.MessagesCollectionInfo);
        }

        public async Task<List<UserInfo>> GetAllUserInfoAsync() =>
            await UserInfo.Find(_ => true).ToListAsync();

        public async Task<UserInfo?> GetUserInfoAsync(string id) =>
            await UserInfo.Find(x => x.UUID == id).FirstOrDefaultAsync();

        public async Task CreateUserInfoAsync(UserInfo newUser) =>
            await UserInfo.InsertOneAsync(newUser);

        public async Task UpdateUserInfoAsync(string id, UserInfo userInfo) =>
            await UserInfo.ReplaceOneAsync(x => x.UUID == id, userInfo);

        public async Task RemoveUserInfoAsync(string id) =>
            await UserInfo.DeleteOneAsync(x => x.UUID == id);



        public async Task<List<UserMessage>> GetUserMessagesAsync() =>
            await UserMessages.Find(_ => true).ToListAsync();

        public async Task<UserMessage?> GetUserMessagesAsync(string id) =>
            await UserMessages.Find(x => x.userUUID == id).FirstOrDefaultAsync();

        public async Task CreateUserMessageAsync(UserMessage newMessage) =>
            await UserMessages.InsertOneAsync(newMessage);

        public async Task UpdateUserMessageAsync(string id, UserMessage userMessage) =>
            await UserMessages.ReplaceOneAsync(x => x.userUUID == id, userMessage);

        public async Task RemoveUserMessageAsync(string id) =>
            await UserMessages.DeleteOneAsync(x => x.userUUID == id);
    }
}

