using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITS_POS.Services
{
    public interface ITokenService
    {
        string GenerateToken();
    }

 }
