using Sometimes.Database.Models;

namespace Sometimes.Database
{
    public interface IDatabaseService
    {
        Task CreateUserInfoAsync(UserInfo newUser);
        Task CreateUserMessageAsync(UserMessage newMessage);
        Task<List<UserInfo>> GetAllUserInfoAsync();
        Task<UserInfo?> GetUserInfoAsync(string id);
        Task<List<UserMessage>> GetUserMessagesAsync();
        Task<UserMessage?> GetUserMessagesAsync(string id);
        Task RemoveUserInfoAsync(string id);
        Task RemoveUserMessageAsync(string id);
        Task UpdateUserInfoAsync(string id, UserInfo userInfo);
        Task UpdateUserMessageAsync(string id, UserMessage userMessage);
    }
}