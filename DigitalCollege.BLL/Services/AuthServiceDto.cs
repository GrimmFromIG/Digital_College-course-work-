using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DigitalCollege.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public AuthResponseDto Authenticate(LoginDto loginDto)
        {
            var user = _unitOfWork.Users.Find(u => u.Email == loginDto.Email).FirstOrDefault();

            if (user == null || user.PasswordHash != loginDto.Password)
            {
                throw new BusinessLogicException("Невірний email або пароль.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            string fullName = "Користувач";
            if (user.Role == "Manager") 
            {
                fullName = "Головний Менеджер";
            }
            else if (user.TeacherId.HasValue) 
            {
                var teacher = _unitOfWork.Teachers.GetById(user.TeacherId.Value);
                if (teacher != null) fullName = teacher.FullName;
            }
            else if (user.StudentId.HasValue) 
            {
                var student = _unitOfWork.Students.GetById(user.StudentId.Value);
                if (student != null) fullName = student.FullName;
            }

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Role = user.Role,
                TeacherId = user.TeacherId,
                StudentId = user.StudentId,
                FullName = fullName
            };
        }
    }
}