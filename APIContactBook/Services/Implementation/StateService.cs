using APIContactBook.Data.Contract;
using APIContactBook.Dtos;
using APIContactBook.Models;
using APIContactBook.Services.Contract;

namespace APIContactBook.Services.Implementation
{
    public class StateService: IStateService
    {

        private readonly IStateRepository _stateRepository;
        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public ServiceResponse<IEnumerable<StateDto>> GetAllStates()
        {
            var response = new ServiceResponse<IEnumerable<StateDto>>();
            var states = _stateRepository.GetAllStates();
            if (states != null && states.Any())
            {
                List<StateDto> stateDtos = new List<StateDto>();
                foreach (var state in states)
                {
                    stateDtos.Add(new StateDto()
                    {
                        StateId=state.StateId,
                        StateName=state.StateName,
                        CountryId = state.CountryId,
                        Country = new Country
                        {
                            CountryId = state.Country.CountryId,
                            CountryName = state.Country.CountryName,
                        }
                    });
                }
                response.Data = stateDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }
        public ServiceResponse<IEnumerable<StateDto>> GetStatesByCountryId(int countryId)
        {
            var response = new ServiceResponse<IEnumerable<StateDto>>();
            var states = _stateRepository.GetStatesByCountryId(countryId);
            if (states != null && states.Any())
            {
                List<StateDto> stateDtos = new List<StateDto>();
                foreach (var state in states)
                {
                    stateDtos.Add(new StateDto()
                    {
                        StateId = state.StateId,
                        StateName = state.StateName,
                        CountryId = state.CountryId,
                        Country = new Country
                        {
                            CountryId = state.Country.CountryId,
                            CountryName = state.Country.CountryName,
                        }
                    });
                }
                response.Data = stateDtos;
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
