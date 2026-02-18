using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            return await _db.Users
                .AnyAsync(u =>
                u.Username == username &&
                u.PasswordHash == password);
        }
    }
}



