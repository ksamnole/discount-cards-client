using System.Windows.Input;
using Client.Models;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IRequestModel _requestModel;
        public ICommand SendRequestCommand { get; private set; }
        public MainPageViewModel()
        {
            SendRequestCommand = new Command(SendRequest);
            _requestModel = new RequestModel();
        }

        private void SendRequest()
        {
            _requestModel.SendRequest();
        }
    }
}