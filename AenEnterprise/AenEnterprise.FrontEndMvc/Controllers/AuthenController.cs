using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System;
using AenEnterprise.FrontEndMvc.Models;
using AenEnterprise.ServiceImplementations.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AenEnterprise.ServiceImplementations.Messaging.Users;
using AenEnterprise.DomainModel.UserDomain;
using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DataAccess;
using Azure.Core;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using AenEnterprise.ServiceImplementations.Mapping;
using System.Text;

namespace AenEnterprise.FrontEndMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private IUnitOfWork _uow;
        public readonly AenEnterpriseDbContext _context;

        public AuthenController(IConfiguration configuration,
            IUserService userService,
            IUnitOfWork uow,
            AenEnterpriseDbContext context)
        {
            _configuration = configuration;
            _userService = userService;
            _uow = uow;
            _context = context;
        }



        [HttpGet("GetUser"), Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }
        [HttpPost("GetUserName")]
        public async Task<ActionResult>GetUserNameForm(UserFormRequest request)
        {
            var user = await _userService.GetUserByName(request.Username);
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserFormRequest request)
        {
            GetAllUserResponse response = new GetAllUserResponse();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                Remove(CookieDataKey.UserName.ToString());
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                CreateUserRequest userRequest = new CreateUserRequest();
                userRequest.Username = request.Username;
                userRequest.Password = request.Password;
                userRequest.PasswordHash = passwordHash;
                userRequest.PasswordSalt = passwordSalt;
                response = await _userService.CreateUser(userRequest);
            }

            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserFormRequest request)
        {
            try
            {
                Remove(CookieDataKey.UserName.ToString());

                var user = await _userService.GetUserByName(request.Username);
                if (user == null || user.User.Username != request.Username)
                {
                    return BadRequest("User not found.");
                }

                if (!VerifyPasswordHash(request.Password, user.User.PasswordHash, user.User.PasswordSalt))
                {
                    return BadRequest("Wrong password.");
                }

                string token = await CreateToken(user.User.Username);

                var refreshToken = GenerateRefreshToken();
                refreshToken.UserName = request.Username;

                UpdateUserRequest loginRequest = new UpdateUserRequest();
                loginRequest.RefreshToken = token;
                loginRequest.Username = request.Username;
                await _userService.UpdateUser(loginRequest);

                Set(CookieDataKey.UserName.ToString(), user.User.Username,1);
                SetRefreshToken(refreshToken);
                return Ok(token);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.ToString());

                // Return a generic error message to the client
                return BadRequest("User not found.");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken(string userName)
        {
            var user =await _userService.GetUserByName(userName);

            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.User.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.User.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = await CreateToken(user.User.Username);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async Task SetRefreshToken(RefreshToken newRefreshToken)
        {
            var user =await _userService.GetUserByName(newRefreshToken.UserName);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.User.RefreshToken = newRefreshToken.Token;
            user.User.TokenCreated = newRefreshToken.Created;
            user.User.TokenExpires = newRefreshToken.Expires;
            user.User.Username = newRefreshToken.UserName;
        }

        private async Task<string> CreateToken(string userName)
        {
            var user =await _userService.GetUserByName(userName);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.User.Username),
                new Claim(ClaimTypes.Role, user.User.RoleName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
               expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds) ; 

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        [HttpGet("GetUser/{Id}")]
        public IActionResult GetUserById(int Id)
        {
            var users = _userService.GetUserById(Id);
            return Ok(users);
        }


        [HttpPost("UpdateUserByUserName")]
        public IActionResult UpdateUser(UserFormRequest user)
        {
            UpdateUserRequest request = new UpdateUserRequest();
            request.RefreshToken = user.RefreshToken;
            request.Username = user.Username;
            _userService.UpdateUser(request);
            return Ok();
        }

        [HttpGet("GetToken")]
        public async Task<IActionResult> GetRefreshToken() {
            string userName = "";
            userName= Get(CookieDataKey.UserName.ToString());
            var token = await _userService.GetRefreshToken(userName);
            return Ok(token);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Remove(CookieDataKey.UserName.ToString());
         
            return Ok();
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.UtcNow.AddHours(24);
            else
                option.Expires = DateTime.UtcNow.AddDays(3);
            Response.Cookies.Append(key, value, option);
        }

        public string Get(string key)
        {
            return Request.Cookies[key];
        }

        public enum CookieDataKey
        {
           UserName
        }

        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
