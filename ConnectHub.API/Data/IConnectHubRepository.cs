using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectHub.API.Models;

namespace ConnectHub.API.Data
{
    public interface IConnectHubRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }
}