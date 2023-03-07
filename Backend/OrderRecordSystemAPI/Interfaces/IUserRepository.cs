using OrderRecordSystemAPI.Models;

namespace OrderRecordSystemAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(string id);
        Task<User?> GetUserByUsername(string username);
        void InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
