using DigitalCollege.BLL.DTOs;

namespace DigitalCollege.BLL.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDto Authenticate(LoginDto loginDto);
    }
}