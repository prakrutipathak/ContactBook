using APIContactBook.Models;

namespace APIContactBook.Data.Contract
{
    public interface IStateRepository
    {
        IEnumerable<State> GetAllStates();
        List<State> GetStatesByCountryId(int id);
    }
}
