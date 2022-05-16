using Client.Entities.Enums;
using Xamarin.Forms;

namespace Client.Entities.Card
{
    public class Card
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public ZXing.BarcodeFormat Standart { get; set; }
        public Shops ShopName { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}