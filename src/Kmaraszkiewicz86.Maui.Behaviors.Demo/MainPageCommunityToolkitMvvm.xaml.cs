using Kmaraszkiewicz86.Maui.Behaviors.Demo.ViewModels;

namespace Kmaraszkiewicz86.Maui.Behaviors.Demo;

public partial class MainPageCommunityToolkitMvvm : ContentPage
{
	public MainPageCommunityToolkitMvvm()
	{
		InitializeComponent();
		BindingContext = new MainCommunityToolkitViewModel();

    }
}