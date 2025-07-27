# Kmaraszkiewicz86.Maui.Behaviors

Kmaraszkiewicz86.Maui.Behaviors is a library for .NET MAUI that provides reusable behaviors to easily attach logic to your UI controls without requiring code-behind. Behaviors help you keep your UI and business logic cleanly separated, making your code more maintainable and testable.

## Features

- Attach logic to any .NET MAUI control declaratively
- Reusable, composable, and testable behaviors
- Supports all .NET MAUI target platforms

## Core Classes

### BehaviorBase

Provides a base class for behaviors that can be attached to `BindableObject` instances.

**Key Methods:**

- **OnAttachedTo**  
  Attaches the behavior to the specified bindable object and initializes the binding context.  
  This method sets the `AssociatedObject` to the specified bindable object and synchronizes the binding context. It also subscribes to the `BindableObject.BindingContextChanged` event to update the binding context when it changes.

- **OnDetachingFrom**  
  Handles the detachment of the behavior from the specified bindable object.  
  This method is called when the behavior is being removed from the bindable object. It unsubscribes from the `BindingContextChanged` event and clears the associated object.

- **OnBindingContextChanged**  
  Called when the binding context changes.  
  Updates the behavior's binding context to match the associated object's binding context.

### EventToCommandBehavior

A behavior that binds an event on a `VisualElement` to an `ICommand`.

**Key Methods:**

- **OnAttachedTo**  
  Called when the behavior is attached to a `VisualElement`.  
  This method is invoked automatically by the framework when the behavior is attached to a visual element. It ensures that the behavior is properly initialized and registers the specified event.

- **OnDetachingFrom**  
  Executes custom logic when the behavior is detached from a `VisualElement`.  
  This method is called automatically when the behavior is removed from a `VisualElement`. Override this method to perform any necessary cleanup or deregistration of events.

- **RegisterEvent**  
  Registers an event handler for the specified event on the associated object.  
  This method attaches an event handler to the event specified by name on the associated object. If the event name is null, empty, or consists only of whitespace, the method returns without performing any action.

- **DeregisterEvent**  
  Deregisters an event handler from the specified event on the associated object.  
  If the event handler is not currently registered or if the event name is null, empty, or whitespace, the method will return without performing any action.

- **OnEvent**  
  Handles the event and executes the associated command.  
  Resolves the command parameter (using the provided parameter, converter, or event args), checks if the command can execute, and then executes it.

- **OnEventNameChanged**  
  Handles changes to the event name property.  
  Deregisters the old event and registers the new event when the event name changes.

## Demo Example

To see how to use behaviors in a real .NET MAUI application, check out the demo implementation:

- [MainPage](Kmaraszkiewicz86.Maui.Behaviors.Demo/MainPage.xaml.cs#L113-L306): The main view that sets up the binding context.
- [MainViewModel](Kmaraszkiewicz86.Maui.Behaviors.Demo/ViewModels/MainViewModel.cs#L621-L4467): The view model that exposes properties and commands for the view, demonstrating how to attach logic using behaviors and data binding.

These examples illustrate how to connect your UI to logic using behaviors and the MVVM pattern in .NET MAUI.
