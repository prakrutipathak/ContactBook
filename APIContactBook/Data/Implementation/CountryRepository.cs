using APIContactBook.Data.Contract;
using APIContactBook.Models;

namespace APIContactBook.Data.Implementation
{
    public class CountryRepository: ICountryRepository
    {
        private readonly IAppDbContext _appDbContext;
        public CountryRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Country> GetAllCountries()
        {
            List<Country> countries = _appDbContext.Countries.ToList();
            return countries;
        }
    }
}
