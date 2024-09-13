using ContactBook.Models;

namespace ContactBook.Data.Contract
{
    public interface IAuthRepository
    {
        bool RegisterUser(User user);
        User? ValidateUser(string username);
        bool UserExist(string loginId, string email);
    }
}
