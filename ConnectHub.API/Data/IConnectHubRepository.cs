using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectHub.API.Helpers;
using ConnectHub.API.Models;

namespace ConnectHub.API.Data
{
    public interface IConnectHubRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Like> GetLike(int userId, int recepientId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}