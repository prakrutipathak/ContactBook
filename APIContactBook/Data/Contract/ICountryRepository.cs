using APIContactBook.Models;

namespace APIContactBook.Data.Contract
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAllCountries();
    }
}
