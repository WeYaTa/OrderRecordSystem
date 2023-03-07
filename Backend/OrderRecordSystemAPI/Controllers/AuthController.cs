using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderRecordSystemAPI.DTOs;
using OrderRecordSystemAPI.Helpers;
using OrderRecordSystemAPI.Interfaces;
using OrderRecordSystemAPI.Models;
using OrderRecordSystemAPI.Repositories;

namespace OrderRecordSystemAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly AppDbContext _db;
        private readonly UserRepository _userRepo;

        public AuthController(IConfiguration config, AppDbContext context, IUserService userService)
        {
            _config = config;
            _db = context;
            _userRepo = new UserRepository(context);
            _userService = userService;
        }

        // POST: auth/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            try
            {
                AuthHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                using var trx = await _db.Database.BeginTransactionAsync();
                var user = new User()
                {
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                _userRepo.InsertUser(user);
                await _userRepo.SaveAsync(_userService.GetUsername());
                await trx.CommitAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // POST: auth/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            try
            {
                using var trx = await _db.Database.BeginTransactionAsync();
                var user = await _userRepo.GetUserByUsername(request.Username);

                if (user == null)
                {
                    ModelState.AddModelError("error", "User not found.");
                    return BadRequest(ModelState);
                }

                if (!AuthHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    ModelState.AddModelError("error", "Invalid credentials.");
                    return BadRequest(ModelState);
                }

                var expireDate = DateTime.UtcNow.AddDays(1);
                user.AccessToken = AuthHelper.CreateToken(user, _config, expireDate);
                user.AccessTokenCreated = DateTime.UtcNow;
                user.AccessTokenExpires = expireDate;

                _userRepo.UpdateUser(user);
                await _userRepo.SaveAsync(_userService.GetUsername());
                await trx.CommitAsync();

                return Ok(user.AccessToken);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
