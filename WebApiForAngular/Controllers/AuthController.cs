using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiForAngular.Data;
using WebApiForAngular.Dtos;
using WebApiForAngular.Models;

namespace WebApiForAngular.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        IAuthRepository _Authrepo;
        IConfiguration _configuration;
        public AuthController(IAuthRepository Authrepo,IConfiguration configuration)
        {
            _Authrepo = Authrepo;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            if(await _Authrepo.UserExists(userForRegisterDto.UserName))
            {
                ModelState.AddModelError("UserName", "UserName Already Exists");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User UserToCreate = new User
            {
                UserName = userForRegisterDto.UserName
            };

            User CreatedUser = await _Authrepo.Register(UserToCreate, userForRegisterDto.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {
            User user = await _Authrepo.Login(userForLoginDto.UserName, userForLoginDto.Password);
            if (user == null)
                return Unauthorized();
            var tokenHandlar = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName)

                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandlar.CreateToken(tokenDescriptor);

            var tokenString = tokenHandlar.WriteToken(token);

            return Ok(tokenString);
        }
    }
}