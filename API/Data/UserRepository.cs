using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        async Task<AppUser> IUserRepository.GetUserByIdAsyc(int id)
        {
            return await _context.Users.FindAsync(id);
        }

       async Task<AppUser> IUserRepository.GetUserByusernameAsync(string username)
        {
            return await _context.Users
            .Include(p=> p.Photos)
            .SingleOrDefaultAsync(x=> x.Username == username);
        }

        async Task<IEnumerable<AppUser>> IUserRepository.GetUsersAsync()
        {
            return await _context.Users
            .Include(p=> p.Photos)
            .ToListAsync();
        }

       async Task<bool> IUserRepository.SaveAllAsync()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        void IUserRepository.Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}