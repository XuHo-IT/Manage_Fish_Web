using AutoMapper;
using Fish_Manage.Models;
using Fish_Manage.Models.DTO.FaceBook;
using Fish_Manage.Models.DTO.Password;
using Fish_Manage.Models.DTO.User;
using Fish_Manage.Repository;
using Fish_Manage.Repository.DTO;
using Fish_Manage.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Fish_Manage.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly FishManageContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailSender _emailSender;
        private readonly APIResponse _response = new();

        public UserController(FishManageContext context, IUserRepository userRepo, IMapper mapper, JwtService jwtService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, EmailSender emailSender, APIResponse response)
        {
            _context = context;
            _userRepo = userRepo;
            _mapper = mapper;
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _response = response;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            var userList = await _userRepo.GetAllAsync();

            var userDTOList = new List<UserDTO>();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDTOList.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "customer"
                });
            }

            _response.Result = userDTOList;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var user = await _userRepo.GetUserByUsername(model.UserName);
            if (user == null || !await _userRepo.ValidatePassword(user, model.Password))
            {
                return Unauthorized(new APIResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid username or password" }
                });
            }

            var token = await _jwtService.GenerateToken(user);
            var loginResponse = new LoginResponseDTO
            {
                Token = token,
                User = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Name = user.Name,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "customer"
                }
            };


            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = loginResponse
            });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            if (!_userRepo.IsUniqueUser(model.UserName))
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Username already exists" }
                });
            }

            var user = await _userRepo.Register(model);
            if (user == null)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Registration failed" }
                });
            }

            var token = await _jwtService.GenerateToken(user);
            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = new LoginResponseDTO
                {
                    Token = token,
                    User = new UserDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Name = user.Name,
                        Role = model.Role
                    }
                }
            });
        }

        [HttpPost("login-facebook")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.AccessToken))
                return BadRequest(new { message = "Invalid Facebook token" });

            using var httpClient = new HttpClient();
            var fbValidationUrl = $"https://graph.facebook.com/me?access_token={request.AccessToken}&fields=id,name,email";
            var response = await httpClient.GetAsync(fbValidationUrl);

            if (!response.IsSuccessStatusCode)
                return Unauthorized(new { message = "Invalid Facebook token" });

            var fbUserData = JsonConvert.DeserializeObject<FacebookUserResponseDTO>(await response.Content.ReadAsStringAsync());
            if (fbUserData == null || string.IsNullOrEmpty(fbUserData.Id))
                return Unauthorized(new { message = "Facebook authentication failed" });

            var existingUser = await _userRepo.GetUserByEmail(fbUserData.Email) ?? new ApplicationUser
            {
                UserName = fbUserData.Email,
                Email = fbUserData.Email,
                Name = fbUserData.Name,
                NormalizedEmail = fbUserData.Email.ToUpper(),
                EmailConfirmed = true,
            };
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, fbUserData.Email);

            if (existingUser.Id == null)
            {
                var result = await _userRepo.CreateUserAsync(existingUser);
                if (!result.Succeeded) return BadRequest(new { message = "User creation failed" });

                await _userRepo.AddToRoleAsync(existingUser.Id, "customer");
            }

            var token = await _jwtService.GenerateToken(existingUser);
            return Ok(new { token, userId = existingUser.Id, isAuthenticated = true, isAdmin = false });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var user = await _userRepo.GetAsync(u => u.Id == id);
            if (user == null) return NotFound();

            await _userRepo.RemoveAsync(user);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id) return BadRequest();

            var user = await _userRepo.GetAsync(u => u.Id == id);
            if (user == null) return NotFound();

            user.UserName = updateDTO.UserName;
            user.Name = updateDTO.Name;
            user.Email = updateDTO.Email;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, updateDTO.Role);

            await _userRepo.UpdateAsync(user);

            return NoContent();
        }
        [HttpPost("forgotPass")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Email not found" }
                });
            }

            // Generate Password Reset Token
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string resetLink = $"https://localhost:5173/Login/ResetPass.html?email={user.Email}&token={WebUtility.UrlEncode(token)}";

            string subject = "Password Reset Request";
            string message = $"Click the link below to reset your password:<br/><a href='{resetLink}'>Reset Password</a>";

            await _emailSender.SendEmailAsync(user.Email, subject, message);

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = "Password reset email sent successfully."
            });
        }
        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUser(string id)
        {
            try
            {
                if (id == "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var user = await _userRepo.GetAsync(u => u.Id == id);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var currentRoles = await _userManager.GetRolesAsync(user);

                var userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Role = currentRoles.FirstOrDefault();

                _response.Result = userDTO;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("resetPass")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid email" }
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList()
                });
            }

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = "Password has been successfully reset."
            });
        }
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext
        //        .AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    var claim = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //    {
        //        claim.Issuer,
        //        claim.OriginalIssuer,
        //        claim.Type,
        //        claim.Value
        //    });
        //    return Ok(new APIResponse
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        IsSuccess = true,
        //        Result = claim
        //    });
        //}

    }
}
