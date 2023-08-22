using Captureit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Captureit.Repository
{
    public interface IUser
    {
        Task<List<User>> GetAllUserAsync();
    }
}
