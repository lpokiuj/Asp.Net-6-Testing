using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testt.Config;
using testt.Models;

namespace testt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
       

        [HttpPost("loginDefaultJwt")]
        public IActionResult LoginDefaultJwt([FromBody] User user)
        {
            var constr = DbSingleton.Configuration.GetSection("credential");
            string usernameCredential = constr.GetValue<string>("username");
            string passwordCredential = constr.GetValue<string>("password");

            if (user.Username != usernameCredential || user.Password != passwordCredential)
            {
                return BadRequest(new { Message = "Wrong Username or Password" });
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@1"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:7208/",
                audience: "https://localhost:7208/",
                claims: new List<Claim>() { new Claim(ClaimTypes.Name, user.Username ?? string.Empty) },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { Token = tokenString });
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("forbidden")]
        public IActionResult GetForbidden()
        {
            return Forbid();
        }
    }
}
