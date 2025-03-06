using AutoMapper;
using Fish_Manage.Models;
using Fish_Manage.Models.DTO.User;
using Fish_Manage.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fish_Manage.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly FishManageContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(FishManageContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager) : base(db)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _configuration = configuration;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user);
        }
        public async Task<bool> AddToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email == email);
            return user;
        }
        public async Task<ApplicationUser> GetUserByUsername(string userName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == userName || x.Email == userName);
            return user;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDTO.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
            {
                return new LoginResponseDTO
                {
                    Token = "",
                    User = null
                };
            }


            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("UserId", user.Id)
    };

            if (roles.Any())
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.First()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                User = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.Name,
                    Role = roles.FirstOrDefault() ?? "customer"
                },

            };
        }
        public async Task<bool> ValidatePassword(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }


        public async Task<ApplicationUser> Register(RegisterationRequestDTO model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                NormalizedEmail = model.Email.ToUpper(),
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            await _userManager.AddToRoleAsync(user, model.Role ?? "customer");
            return user;
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
