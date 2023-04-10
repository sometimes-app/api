using Sometimes.Database;
using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IDatabaseService DatabaseService;

        public UserInfoService(IDatabaseService databaseService)
        {
            DatabaseService = databaseService;
        }

        /// <inheritdoc/>
        public async Task<UserInfo?> GetUserInfo(string uuid)
        {
            IsValidGuid(uuid);

            var user = await DatabaseService.GetUserInfoAsync(uuid);

            return user;
        }

        /// <inheritdoc/>
        public async Task<UserInfo?> CreateUserInfo(UserInfo newUser)
        {
            IsValidGuid(newUser.UUID);

            await DatabaseService.CreateUserInfoAsync(newUser);
            var user = await DatabaseService.GetUserInfoAsync(newUser.UUID);
            return user;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FriendInfo>> GetFriends(string uuid)
        {
            IsValidGuid(uuid);

            var friendsUserInfo = await DatabaseService.GetFriends(uuid);
            if (friendsUserInfo == null)
            {
                return new List<FriendInfo>();
            }

            return friendsUserInfo.Select(friend => new FriendInfo
            {
                UUID = friend.UUID,
                FirstName = friend.FirstName,
                LastName = friend.LastName,
                UserName = friend.UserName,
                ProfilePicUrl = friend.ProfilePicUrl
            }).AsEnumerable();
        }

        /// <inheritdoc/>
        public async Task<bool> AddFriend(string userUuid, string friendUuid)
        {
            IsValidGuid(userUuid);
            IsValidGuid(friendUuid);

            return await DatabaseService.AddFriend(userUuid, friendUuid);

        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFriend(string userUuid, string friendUuid)
        {
            IsValidGuid(userUuid);
            IsValidGuid(friendUuid);

            return await DatabaseService.RemoveFriend(userUuid, friendUuid);
        }

        /// <summary>
        /// Checks whether a string is a valid GUID
        /// </summary>
        /// <param name="guid">GUID to check vaildity of</param>
        /// <returns><paramref name="guid"/> as a GUID</returns>
        /// <exception cref="ArgumentException">Throws if the GUID is an invalid format</exception>
        private Guid IsValidGuid(string guid)
        {
            Extensions.ValidatedNullable(guid);

            var isGuid = Guid.TryParse(guid, out Guid validGuid);

            if (isGuid)
            {
                return validGuid;
            }
            else
            {
                throw new ArgumentException($"Invalid UUID: {guid}");
            }
        }
    }
}

