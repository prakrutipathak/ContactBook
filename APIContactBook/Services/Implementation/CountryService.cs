using APIContactBook.Data.Contract;
using APIContactBook.Dtos;
using APIContactBook.Services.Contract;

namespace APIContactBook.Services.Implementation
{
    public class CountryService: ICountryService
    {

        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public ServiceResponse<IEnumerable<CountryDto>> GetCountries()
        {
            var response = new ServiceResponse<IEnumerable<CountryDto>>();
            var countries = _countryRepository.GetAllCountries();
            if (countries != null && countries.Any())
            {
                List<CountryDto> countryDtos = new List<CountryDto>();
                foreach (var country in countries)
                {
                    countryDtos.Add(new CountryDto()
                    {  
                            CountryId = country.CountryId,
                            CountryName = country.CountryName,
                    });
                }
                response.Data = countryDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }
    }
}
