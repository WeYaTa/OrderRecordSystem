using Microsoft.EntityFrameworkCore;
using OrderRecordSystemAPI.Interfaces;
using OrderRecordSystemAPI.Models;

namespace OrderRecordSystemAPI.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly AppDbContext _db;
        private bool _disposed;

        public UserRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<User?> GetUserById(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public void InsertUser(User user)
        {
            _db.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
        }

        public void DeleteUser(User user)
        {
            _db.Remove(user);
        }

        public async Task SaveAsync(string username)
        {
            await _db.SaveChangesAsync(username);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
