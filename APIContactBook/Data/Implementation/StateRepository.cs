using APIContactBook.Data.Contract;
using APIContactBook.Models;
using Microsoft.EntityFrameworkCore;

namespace APIContactBook.Data.Implementation
{
    public class StateRepository : IStateRepository
    {
        private readonly IAppDbContext _appDbContext;
        public StateRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<State> GetAllStates()
        {
            List<State> states = _appDbContext.States.Include(c=>c.Country).ToList();
            return states;
        }
        public List<State> GetStatesByCountryId(int id)
        {
            List<State> countries = _appDbContext.States.Include(p => p.Country).Where(c => c.CountryId == id).ToList();
            return countries;
        }
    }
}
