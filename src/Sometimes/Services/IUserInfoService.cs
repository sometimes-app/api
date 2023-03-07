using Sometimes.Database.Models;

namespace Sometimes.Services
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
        Task<UserInfo?> PutUserInfo(UserInfo newUser);
    }
}