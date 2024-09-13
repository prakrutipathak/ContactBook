using APIContactBook.Models;
using APIContactBook.Services.Implementation;

namespace APIContactBook.Services.Contract
{
    public interface IVerifyPasswordHash
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
