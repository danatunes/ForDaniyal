
using Core.Dtos;
using Core.Models;
using Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication17.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;


        public AuthController(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterDto userRegisterDTO)
        {
            userRegisterDTO.Username = userRegisterDTO.Username.ToLower();

            if (_authRepository.UserExists(userRegisterDTO.Username))
                return BadRequest("Username already exists!");

            var userToCreate = new User
            {
                Name = userRegisterDTO.Name,
                Username = userRegisterDTO.Username,
                Role = userRegisterDTO.Role
            };

            var createdUser = _authRepository.Register(userToCreate, userRegisterDTO.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userLoginDTO)
        {
            var user = _authRepository.Login(userLoginDTO.Username.ToLower(), userLoginDTO.Password);

            if (user == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, ClaimsIdentity.DefaultNameClaimType),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
