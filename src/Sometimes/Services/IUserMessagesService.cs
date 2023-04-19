using Sometimes.Database.Models;
using Sometimes.Models;

namespace Sometimes.Services
{
    public interface IUserMessagesService
    {
        /// <summary>
        /// Gets the users daily message with given uuid
        /// </summary>
        /// <param name="uuid">User's uuid</param>
        /// <returns><see cref="Message"/></returns>
        Task<DisplayMessage?> GetDailyMessage(string uuid);
        /// <summary>
        /// Sets read message flag to true for a given messageID
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true for success false for not found</returns>
        Task<bool> ReadMessageAsync(string messageID);
    }
}
