namespace Client.Entities.Card
{
    public class CardEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public string Number { get; set; }
        public int Standart { get; set; }
    }
}