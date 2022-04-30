namespace Client.Entities
{
    public class CardEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string ImageSource { get; set; }
    }
}