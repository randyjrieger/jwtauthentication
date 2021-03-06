using JWTAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserDTO> users = new List<UserDTO>();
        public UserDTO GetUser(UserModel userModel)
        {
            return users.Where(x => x.UserName.ToLower() == userModel.UserName.ToLower() && x.Password == userModel.Password).FirstOrDefault();
        }

        public UserRepository()
        {
            users.Add(new UserDTO
            {
                UserName = "miranda",
                Password = "chicken",
                Role = "dev"
            });

            users.Add(new UserDTO
            {
                UserName = "lilly",
                Password = "flower",
                Role = "admin"
            });

            users.Add(new UserDTO
            {
                UserName = "darcy",
                Password = "weirdo",
                Role = "dev"
            });
        }
    }
}
