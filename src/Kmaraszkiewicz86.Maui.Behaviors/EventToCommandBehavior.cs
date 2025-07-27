using System.Reflection;
using System.Windows.Input;

namespace Kmaraszkiewicz86.Maui.Behaviors
{
    /// <summary>
    /// A behavior that binds an event on a <see cref="VisualElement"/> to an <see cref="ICommand"/>.
    /// </summary>
    /// <remarks>This behavior allows you to execute a command in response to an event raised by a <see
    /// cref="VisualElement"/>.  You can specify the event name, the command to execute, an optional command parameter,
    /// and an optional value converter  to transform the event arguments before passing them to the command.</remarks>
    public class EventToCommandBehavior : BehaviorBase<VisualElement>
    {
        /// <summary>
        /// Represents a delegate that serves as the event handler for an event.
        /// </summary>
        /// <remarks>This delegate can be assigned to handle events and execute the associated logic when
        /// the event is raised. Ensure that the delegate is properly initialized before use.</remarks>
        Delegate eventHandler = default!;

        /// <summary>
        /// Identifies the bindable property for the name of the event to be observed.
        /// </summary>
        /// <remarks>This property is used to specify the name of the event that the behavior will listen
        /// to. When the event is triggered, the associated command will be executed.</remarks>
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        /// <summary>
        /// Identifies the bindable property for the command to be executed when the associated event is triggered.
        /// </summary>
        /// <remarks>This property is used to bind an <see cref="ICommand"/> to the behavior, allowing the
        /// execution of the command in response to the specified event. The command can be parameterized using the <see
        /// cref="CommandParameterProperty"/>.</remarks>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
        /// <summary>
        /// Identifies the bindable property for the command parameter associated with the <see
        /// cref="EventToCommandBehavior"/>.
        /// </summary>
        /// <remarks>This property is used to bind a parameter that will be passed to the command executed
        /// by the <see cref="EventToCommandBehavior"/>.</remarks>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);
        /// <summary>
        /// Identifies the bindable property used to specify the input converter for the behavior.
        /// </summary>
        /// <remarks>The input converter is used to transform the event arguments into a format suitable
        /// for the command parameter. This property can be set to an implementation of <see cref="IValueConverter"/> to
        /// customize the conversion logic.</remarks>
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        /// <summary>
        /// Gets or sets the name of the event associated with this instance.
        /// </summary>
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to be executed when the associated action is triggered.
        /// </summary>
        /// <remarks>The command should implement the <see cref="ICommand"/> interface, which provides the
        /// <see cref="ICommand.Execute(object)"/> and <see cref="ICommand.CanExecute(object)"/> methods  to define the
        /// execution logic and enable/disable state of the command.</remarks>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the command when it is executed.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        /// <summary>
        /// Gets or sets the converter used to transform the input value.   
        /// </summary>
        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        /// <summary>
        /// Called when the behavior is attached to a <see cref="VisualElement"/>.
        /// </summary>
        /// <remarks>This method is invoked automatically by the framework when the behavior is attached
        /// to a visual element. It ensures that the behavior is properly initialized and registers the specified
        /// event.</remarks>
        /// <param name="bindable">The <see cref="VisualElement"/> to which the behavior is being attached.</param>
        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        /// <summary>
        /// Executes custom logic when the behavior is detached from a <see cref="VisualElement"/>.
        /// </summary>
        /// <remarks>This method is called automatically when the behavior is removed from a <see
        /// cref="VisualElement"/>. Override this method to perform any necessary cleanup or deregistration of
        /// events.</remarks>
        /// <param name="bindable">The <see cref="VisualElement"/> from which the behavior is being detached.</param>
        protected override void OnDetachingFrom(VisualElement bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        /// <summary>
        /// Registers an event handler for the specified event on the associated object.
        /// </summary>
        /// <remarks>This method attaches an event handler to the event specified by <paramref
        /// name="name"/> on the associated object. If the event name is null, empty, or consists only of whitespace,
        /// the method returns without performing any action.</remarks>
        /// <param name="name">The name of the event to register. Cannot be null, empty, or whitespace.</param>
        /// <exception cref="ArgumentException">Thrown if the specified event cannot be found on the associated object.</exception>
        void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
            }
            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        /// <summary>
        /// Deregisters an event handler from the specified event on the associated object.
        /// </summary>
        /// <remarks>If the event handler is not currently registered or if the event name is null, empty,
        /// or whitespace, the method will return without performing any action.</remarks>
        /// <param name="name">The name of the event from which to deregister the handler. Cannot be null, empty, or whitespace.</param>
        /// <exception cref="ArgumentException">Thrown if the specified event does not exist on the associated object.</exception>
        void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (eventHandler == null)
            {
                return;
            }
            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
            }
            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
            {
                return;
            }

            object resolvedParameter;

            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (Converter != null)
            {
                resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (Command.CanExecute(resolvedParameter))
            {
                if (resolvedParameter != null && bool.TryParse(resolvedParameter.ToString(), out bool booleanParameter))
                {
                    Command.Execute(booleanParameter);
                }
                else
                {
                    Command.Execute(resolvedParameter);
                }
            }
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}