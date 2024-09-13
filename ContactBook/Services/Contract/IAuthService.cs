using ContactBook.ViewModels;

namespace ContactBook.Services.Contract
{
    public interface IAuthService
    {
        string registerUserService(RegisterViewModel register);
        string LoginUserService(LoginViewModel login);
    }
}
