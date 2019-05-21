using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectHub.API.Helpers;
using ConnectHub.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ConnectHub.API.Data
{
    public class ConnectHubRepository : IConnectHubRepository
    {
        private readonly DataContext _dataContext;
        public ConnectHubRepository(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public void Add<T>(T entity) where T : class
        {
            _dataContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dataContext.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _dataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _dataContext.Users.Include(p => p.Photos)
            .OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId && u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99) {
                
                var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);
            }

            if(!string.IsNullOrWhiteSpace(userParams.OrderBy)) {
                switch(userParams.OrderBy) {
                    case "created":
                    users = users.OrderByDescending(u => u.Created);
                    break;
                    default:
                    users = users.OrderByDescending(u => u.LastActive);
                    break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int id) {
            var photo = await _dataContext.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _dataContext.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}