using Sometimes.Database;
using Sometimes.Database.Models;
using Sometimes.Models;
using Sometimes.Services.Interfaces;

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
            var user = await DatabaseService.GetUserInfoAsync(uuid);

            return user;
        }

        /// <inheritdoc/>
        public async Task<UserInfo?> CreateUserInfo(UserInfo newUser)
        {
            await DatabaseService.CreateUserInfoAsync(newUser);
            var user = await DatabaseService.GetUserInfoAsync(newUser.UUID);
            return user;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FriendInfo>> GetFriends(string uuid)
        {

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
            return await DatabaseService.AddFriend(userUuid, friendUuid);
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFriend(string userUuid, string friendUuid)
        {
            return await DatabaseService.RemoveFriend(userUuid, friendUuid);
        }
    }
}

