using Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCardPage : ContentPage
    {
        private readonly AddCardViewModel _addCardViewModel;
        
        public AddCardPage(AddCardViewModel addCardViewModel)
        {
            InitializeComponent();
            
            _addCardViewModel = addCardViewModel;
            BindingContext = _addCardViewModel;
            
            NavigationPage.SetIconColor(this, Color.Black);
        }

        protected override void OnAppearing()
        {
            _addCardViewModel.UpdateShops();
            base.OnAppearing();
        }
    }
}