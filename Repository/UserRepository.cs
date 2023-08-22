using Captureit.Data;
using Captureit.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Captureit.Repository
{
    public class UserRepository:IUser
    {
        private readonly ApplicationDbContext _Context;
        public UserRepository(ApplicationDbContext context)
        {
            _Context = context;
        }
        public async Task<List<User>> GetAllUserAsync()
        {
            return await _Context.Users.ToListAsync();
        }

        
    }
}
