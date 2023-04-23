using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Database
{
    public interface IDatabaseService
    {
        Task CreateUserInfoAsync(UserInfo newUser);
        Task<List<UserInfo>> GetAllUserInfoAsync();
        Task<UserInfo?> GetUserInfoAsync(string id);
        Task RemoveUserInfoAsync(string id);
        Task UpdateUserInfoAsync(string id, UserInfo userInfo);

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

        /// <summary>
        /// Gets the daily message for a given uuid
        /// </summary>
        /// <param name="uuid">requester's uuid</param>
        /// <returns><see cref="UserMessage"/></returns>
        Task<DisplayMessage?> GetDailyMessage(string uuid);

        /// <summary>
        /// Sets read message flag to true for a given messageID
        /// </summary>
        /// <param name="uuid">user id</param>
        /// <param name="messageID">message id</param>
        /// <returns>true for success false for not found</returns>
        Task<bool> ReadMessage(string uuid, string messageID);
    }
}