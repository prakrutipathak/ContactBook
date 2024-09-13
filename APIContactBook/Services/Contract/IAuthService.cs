using APIContactBook.Dtos;
using APIContactBook.Models;

namespace APIContactBook.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<string> registerUserService(RegisterDto register);
        ServiceResponse<string> LoginUserService(LoginDto login);
        ServiceResponse<string> ForgetPasswordService(ForgetDto forgetDto);
        ServiceResponse<GetUserDto> GetUser(int id);
        ServiceResponse<string> ModifyUser(User user);
    }
}
