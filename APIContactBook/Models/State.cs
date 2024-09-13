namespace APIContactBook.Models
{
    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        //foreign key
        public int CountryId { get; set; }
       
        public Country Country { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
