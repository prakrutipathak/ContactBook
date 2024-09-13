using APIContactBook.Dtos;

namespace APIContactBook.Services.Contract
{
    public interface ICountryService
    {
        ServiceResponse<IEnumerable<CountryDto>> GetCountries();
    }
}
