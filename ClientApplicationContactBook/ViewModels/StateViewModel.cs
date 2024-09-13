using System.Diagnostics.Metrics;

namespace ClientApplicationContactBook.ViewModels
{
    public class StateViewModel
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        //foreign key
        public int CountryId { get; set; }

        public ContactsCountryViewModel Country { get; set; }
    }
}
