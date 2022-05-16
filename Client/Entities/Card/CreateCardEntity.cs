namespace Client.Entities.Card
{
    public class CreateCardEntity
    {
        public string UserLogin { get; set; }
        public int ShopId { get; set; }
        public string Number { get; set; }
        public int Standart { get; set; }
    }
}