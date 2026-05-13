using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Kmaraszkiewicz86.Maui.Behaviors.Demo.ViewModels
{
    /// <summary>
    /// Demo view model to illustrate the use of behaviors in a .NET MAUI application from the Kmaraszkiewicz86.Maui.Behaviors library with the CommunityToolkit.Mvvm library.
    /// </summary>
    public class MainCommunityToolkitViewModel : ObservableObject
    {
        /// <summary>
        /// Check if async logic is finised
        /// </summary>
        public bool IsLoading
        {
            get => field;
            set => SetProperty(ref field, value);
        } = true;

        /// <summary>
        /// Simple message property that shows the behaviors logic will work.
        /// </summary>
        public string Message
        {
            get => field;
            set => SetProperty(ref field, value);
        } = "Loading...";

        /// <summary>
        /// Present simple load command that simulates a loading operation by delaying for a short time before updating the message.
        /// </summary>
        public IAsyncRelayCommand LoadCommand => new AsyncRelayCommand(OnLoadAsync);

        /// <summary>
        /// Present simple load command that simulates a loading operation by delaying for a short time before updating the message.
        /// </summary>
        private async Task OnLoadAsync()
        {
            //to simulate some background work, we will just delay for a short time before updating the message
            await Task.Delay(2000);
            Message = "Application Loaded!";
            IsLoading = false;
        }
    }
}
