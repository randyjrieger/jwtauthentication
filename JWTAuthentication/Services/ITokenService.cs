﻿using JWTAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Services
{
    public interface ITokenService
    {
        string BuildToken(
            string key,
            string issuer, 
            UserDTO user);
        bool ValidateToken(string key, string issuer, string audience, string token);

        bool IsTokenValid(string key, string issuer, string token);
    }
}
