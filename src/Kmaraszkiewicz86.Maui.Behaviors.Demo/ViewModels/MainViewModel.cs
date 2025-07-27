using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Kmaraszkiewicz86.Maui.Behaviors.Demo.ViewModels
{
    /// <summary>
    /// Demo view model to illustrate the use of behaviors in a .NET MAUI application from the Kmaraszkiewicz86.Maui.Behaviors library.
    /// </summary>
    /// <remarks>This class implements <see cref="INotifyPropertyChanged"/> to support data binding and
    /// notifies the UI of property changes. It includes a default message, a command for loading data, and a property
    /// indicating authentication status.</remarks>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Simple message property that shows the behaviors logic will work.
        /// </summary>
        /// <remarks>This field is initialized with the value "Loading..." and is intended to be used
        /// internally  to indicate a loading state. It is not exposed publicly.</remarks>
        private string _message = "Loading...";

        /// <summary>
        /// Simple message property that shows the behaviors logic will work.
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// Gets the command that initiates the loading operation.
        /// </summary>
        /// <remarks>This command is typically used to trigger data loading or other initialization tasks.
        /// Ensure that the command is properly bound and executed within the appropriate context.</remarks>
        public ICommand LoadCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            LoadCommand = new Command(() =>
            {
                Message = "Hello, World!";
            });
        }

        /// <summary>
        /// Sets the value of a property and raises a property change notification if the value has changed.
        /// </summary>
        /// <remarks>This method is typically used in property setters to simplify the implementation of
        /// the <see cref="INotifyPropertyChanged"/> pattern. It compares the current value with the new value using
        /// <see cref="EqualityComparer{T}.Default"/> and only updates the value and raises the <see
        /// cref="OnPropertyChanged(string)"/> event if the values are different.</remarks>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="backingStore">A reference to the field storing the property's current value.</param>
        /// <param name="value">The new value to set for the property.</param>
        /// <param name="propertyName">The name of the property. This is automatically provided by the compiler when called from a property setter.</param>
        /// <returns><see langword="true"/> if the value was changed and the property change notification was raised; otherwise,
        /// <see langword="false"/>.</returns>
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event to notify listeners that a property value has changed.
        /// </summary>
        /// <remarks>This method is typically called from within a property's setter to notify
        /// data-binding clients or other listeners of a property change. Ensure that the <see cref="PropertyChanged"/>
        /// event is subscribed to before invoking this method.</remarks>
        /// <param name="propertyName">The name of the property that changed. This value is optional and will default to the name of the caller if
        /// not explicitly provided, using the <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
