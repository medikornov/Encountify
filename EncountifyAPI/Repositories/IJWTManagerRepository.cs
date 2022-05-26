using EncountifyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncountifyAPI.Repositories
{
    public interface IJWTManagerRepository
    {
        Token Authenticate(UserLogin user);
    }
}
