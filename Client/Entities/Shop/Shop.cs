using SQLite;

namespace Client.Entities.Shop
{
    public class Shop
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}