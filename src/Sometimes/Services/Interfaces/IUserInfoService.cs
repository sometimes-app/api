using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Services.Interfaces
{
    public interface IUserInfoService
    {
        /// <summary>
        /// Gets the User Info given a user uuid
        /// </summary>
        /// <param name="uuid">User's uuid</param>
        /// <returns><see cref="UserInfo"/></returns>
        /// <exception cref="ArgumentException">Throws if <paramref name="uuid"/> is not a valid GUID format</exception>
        Task<UserInfo?> GetUserInfo(string uuid);

        /// <summary>
        /// Creates the user info and returns if successfully added
        /// </summary>
        /// <param name="newUser">New user to create</param>
        /// <returns><see cref="UserInfo"/> or null if unsuccessful</returns>
        /// <exception cref="ArgumentException">Throws if <paramref name="newUser"/> does not contain not a valid GUID format for the UUID</exception>
        Task<UserInfo?> CreateUserInfo(UserInfo newUser);

        /// <summary>
        /// Gets all the friend info based on a list of uuids
        /// </summary>
        /// <param name="uuid">uuid of user to get friends of</param>
        /// <returns>List of <see cref="FriendInfo"/></returns>
        Task<IEnumerable<FriendInfo>> GetFriends(string uuid);

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