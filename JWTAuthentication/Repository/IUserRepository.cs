using JWTAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Repository
{
    public interface IUserRepository
    {
        UserDTO GetUser(UserModel userModel);
    }
}
