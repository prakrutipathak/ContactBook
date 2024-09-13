using APIContactBook.Dtos;

namespace APIContactBook.Services.Contract
{
    public interface IStateService
    {
        ServiceResponse<IEnumerable<StateDto>> GetAllStates();
        ServiceResponse<IEnumerable<StateDto>> GetStatesByCountryId(int countryId);
    }
}
