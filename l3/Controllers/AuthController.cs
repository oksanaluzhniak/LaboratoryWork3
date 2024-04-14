using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace l3.Controllers
{
    public class AuthController : Controller
    {
        private readonly Users _users;
        public AuthController(Users users)
        {
            _users = users;
        }
        public static string GetToken(string username)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
            claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
            };

            return response.ToString();

        }
        [HttpPost("/addlogin")]
        public async Task<IActionResult> AddLogin()
        {

            var request = Request.Body;
            string body;
            using (StreamReader reader = new StreamReader(request))
            {
                body = await reader.ReadToEndAsync();
            }
            List<User> form = JsonSerializer.Deserialize<List<User>>(body);
            int count = form.Count;
            int countadds = 0;
            for (int i = 0; i < count; i++)
            {

                bool ans = _users.AddUser(form[i]);
                if (ans == true)
                {
                    countadds++;
                }

            }
            return Json(new { Status = $"Added {countadds} credentials" });

        }
        [Route("/creadentials")]
        public IActionResult ListOfCredentials()
        {
            if (_users != null)
            {
                return Json(_users.DataUsers
                    );
            }
            else
            {
                return Ok("Missing data");
            }
        }
        [HttpGet("/token/{user}/{password}")]
        public IActionResult Token(string user, string password)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                return Json(new { Error = "Credentials are empty" });
            }

            // Перевірка логіна і пароля (регістронезалежний пошук)
            var userFromDb = _users.CheckLogin(user);
            if (userFromDb != null && userFromDb.Password==password)
            {
                // Відповідь з токеном
                return Json(JsonSerializer.Serialize(GetToken(user)));
            }
            else
            {
                return Json(new { Error = "Credentials are empty" });
            }
        }


    }
}
