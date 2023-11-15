using Microsoft.AspNetCore.Mvc;
using Services.Services.RegisterServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<UserDto> Login(LoginDto loginDto);
       // Task<UserDto> GetCurrentUser();
    }
}
