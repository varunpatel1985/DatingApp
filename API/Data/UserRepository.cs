using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers;
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

        async Task<PagedList<AppUser>> IUserRepository.GetUsersAsync(UserParams userParams)
        {
            var query = _context.Users
            .Include(p=> p.Photos)
            .AsNoTracking()
            .AsQueryable();

            query = query.Where(u=> u.Username != userParams.CurrentUsername);
            query = query.Where(u=> u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge -1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u=> u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            query = userParams.OrderBy switch 
            {
                "created" => query.OrderByDescending(u=> u.Created),
                _=> query.OrderByDescending(u=> u.LastActive)
            };
           

            return await PagedList<AppUser>.CreateAsync(query, userParams.PageNumber,userParams.PageSize);
           
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