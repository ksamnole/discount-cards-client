using SQLite;

namespace Client.Entities.Card
{
    [Table("Cards")]
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Number { get; set; }
        public ZXing.BarcodeFormat Standart { get; set; }
        public string ShopName { get; set; }
        public string ImageSource { get; set; }
        public bool IsSync { get; set; }
        public double LastDistanceToShop { get; set; }
    }
}