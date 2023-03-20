using System;
using Sometimes.Database;
using Sometimes;
using Sometimes.Database.Models;

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
            var validUuid = IsValidGuid(uuid);

            var user = await DatabaseService.GetUserInfoAsync(validUuid.ToString());

            return user;
        }

        /// <inheritdoc/>
        public async Task<UserInfo?> PutUserInfo(UserInfo newUser)
        {
            IsValidGuid(newUser.UUID); // TODO: Create/Update API Model to take GUID as datatype instead of string

            await DatabaseService.CreateUserInfoAsync(newUser);
            var user = await DatabaseService.GetUserInfoAsync(newUser.UUID);
            return user;
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

