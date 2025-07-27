using Kmaraszkiewicz86.Maui.Behaviors.Demo.ViewModels;

namespace Kmaraszkiewicz86.Maui.Behaviors.Demo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }

}
