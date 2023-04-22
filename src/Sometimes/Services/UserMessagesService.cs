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
            var message = await DatabaseService.GetDailyMessage(uuid);
            if (message == null)
            {
                return null;
            }

            var result = new DisplayMessage
            {
                MessageID = message.messageId,
                MessageBody = message.body,
            };
            if (!string.IsNullOrEmpty(message.senderUuid))
            {
                var senderInfo = await DatabaseService.GetUserInfoAsync(message.senderUuid);
                if (senderInfo != null)
                    result.SenderName = senderInfo.FirstName + " " + senderInfo.LastName;
            }
            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> ReadMessage(string uuid, string messageID) => await DatabaseService.ReadMessage(uuid, messageID);
    }
}
