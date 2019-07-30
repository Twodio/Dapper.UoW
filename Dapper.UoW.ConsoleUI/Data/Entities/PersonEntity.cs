namespace Dapper.UoW.ConsoleUI.Data.Entities
{
    public class PersonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Address_id { get; set; }
        public AddressEntity Address { get; set; }
    }
}
