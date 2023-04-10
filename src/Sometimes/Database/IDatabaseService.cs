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

        /// <summary>
        /// Returns a list of user info based off a single user's uuid
        /// </summary>
        /// <param name="uuid">user uuid to get friends of</param>
        /// <returns>A list of friends as <see cref="UserInfo"/></returns>
        Task<List<UserInfo>?> GetFriends(string uuid);

        /// <summary>
        /// Adds a friend to the given user
        /// </summary>
        /// <param name="userUuid">user uuid to add friend to</param>
        /// <param name="friendUuid">friend to add</param>
        /// <returns>True if acknowledged, false otherwise</returns>
        Task<bool> AddFriend(string userUuid, string friendUuid);

        /// <summary>
        /// Removes a friend from the given user
        /// </summary>
        /// <param name="userUuid">user uuid to add friend to</param>
        /// <param name="friendUuid">friend to add</param>
        /// <returns>True if acknowledged, false otherwise</returns>
        Task<bool> RemoveFriend(string userUuid, string friendUuid);

    }
}