using Sometimes.Database;
using Sometimes.Database.Models;
using Sometimes.Models;
using Sometimes.Services.Interfaces;

namespace Sometimes.Services
{
    public class UserMessagesService : IUserMessagesService
    {
        private readonly IDatabaseService DatabaseService;

        public UserMessagesService(IDatabaseService databaseService)
        {
            DatabaseService = databaseService;
        }

        /// <inheritdoc/>
        public async Task<DisplayMessage?> GetDailyMessage(string uuid)
        {
            return await DatabaseService.GetDailyMessage(uuid);
        }

        /// <inheritdoc/>
        public async Task<bool> ReadMessage(string uuid, string messageID) => await DatabaseService.ReadMessage(uuid, messageID);
    }
}
