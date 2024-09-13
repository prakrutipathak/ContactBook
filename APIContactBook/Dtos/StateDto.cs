using APIContactBook.Models;

namespace APIContactBook.Dtos
{
    public class StateDto
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        //foreign key
        public int CountryId { get; set; }

        public Country Country { get; set; }
    }
}
