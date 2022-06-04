using SQLite;

namespace Client.Entities.Card
{
    public class DeletedCard
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Number { get; set; }
    }
}