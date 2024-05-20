

using NetPCTask.Data;
using NetPCTask.Models;

namespace NetPCTask.Services
{
    public class AccountService
    {
        private readonly AppDbContext _appDbContext;

        public AccountService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User Register(User user)
        {
            var existingUser = _appDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return null;
            }

            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();

            return user;
        }

        public User Create(User user)
        {
            _appDbContext.Users.Add(user);
            user.Id = _appDbContext.SaveChanges();
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return _appDbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int id)
        {
            return _appDbContext.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
