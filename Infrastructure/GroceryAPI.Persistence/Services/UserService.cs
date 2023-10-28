
using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Abstractions.Token;
using GroceryAPI.Application.DTOs;
using GroceryAPI.Application.DTOs.User;
using GroceryAPI.Application.Exceptions;
using GroceryAPI.Application.Helpers;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Domain.Entities.Identity;
using GroceryAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;
        readonly IEndpointReadRepository _endpointReadRepository;
        readonly RoleManager<AppRole> _roleManager;
        readonly IHttpContextAccessor _contextAccessor;
        readonly ITokenHandler _tokenHandler;

        public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository, RoleManager<AppRole> roleManager, IHttpContextAccessor contextAccessor, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
            _tokenHandler = tokenHandler;
        }

        public int TotalUsersCount => _userManager.Users.Count();

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            AppUser user = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
            {
                response.Message = "User created successfully!";
                // Assign UserRole as a default role when a new user added.
                var defaultrole = _roleManager.FindByNameAsync("UserRole").Result;
                if (defaultrole != null)
                {
                    await _userManager.AddToRoleAsync(user, "UserRole");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}";
                }
            }

            return response;
        }

        public async Task<CreateUserResponse> UpdateAsync(UpdateUser model)
        {
            AppUser user = null;
            user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                user = await _userManager.FindByNameAsync(model.Username);
                user.Email = model.Email;
                if (user == null) throw new NotFoundUserException();
            }
            bool isChange = user.UserName != model.Username;
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Username;
            IdentityResult result = await _userManager.UpdateAsync(user);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
            {
                response.Message = "User updated successfully!";
                _contextAccessor.HttpContext.Items["User"] = user.UserName;

                // Token'ı güncelle
                if (isChange)
                {
                    string[] userRoles = (await _userManager.GetRolesAsync(user)).ToArray();
                    Token newToken = _tokenHandler.CreateAccessToken(2000, user, userRoles[0]);
                    await UpdateRefreshTokenAsync(newToken.RefreshToken, user, newToken.Expiration, 500);
                    response.AccessToken = newToken.AccessToken; // Yeni token'ı istemciye gönder
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}";
                }
            }

            return response;
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();
            }
        }

        public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
        {
            List<AppUser>? users = null;
            if (page == -1 && size == -1)
                users = await _userManager.Users.ToListAsync();
            else
                users = await _userManager.Users
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return users.Select(user => new ListUser
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                TwoFactor = user.TwoFactorEnabled,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber
            }).ToList();
        }

        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            AppUser user = await _userManager.FindByIdAsync(userIdOrName);
            var userRoles = new string[] { };
            if(user != null)
            {
                userRoles = (await _userManager.GetRolesAsync(user)).ToArray();
            }
            else
            {
                user = await _userManager.FindByNameAsync(userIdOrName);
                if (user != null)
                {
                    userRoles = (await _userManager.GetRolesAsync(user)).ToArray();
                }
            }
            return userRoles;
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesToUserAsync(name);
            if (!userRoles.Any())
                return false;

            Domain.Entities.Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code);

            if (endpoint == null)
                return false;

            var hasRole = false;
            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            foreach (var userRole in userRoles)
            {
                    foreach (var endpointRole in endpointRoles)
                        if (userRole == endpointRole)
                            return true;
            }

            return false;
        }

        public async Task RemoveUserAsync(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListUser> GetUserByNameAsync(string name)
        {
            AppUser user = await _userManager.FindByNameAsync(name);
            if(user != null)
            {
                return new ()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    TwoFactor = user.TwoFactorEnabled,
                    Username = user.UserName,
                    PhoneNumber = user.PhoneNumber
                };
            }
            else
            {
                user = await _userManager.FindByEmailAsync(name);
                return new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    TwoFactor = user.TwoFactorEnabled,
                    Username = user.UserName,
                    PhoneNumber = user.PhoneNumber
                };
            }
            throw new NotFoundUserException();
        }

    }
}
