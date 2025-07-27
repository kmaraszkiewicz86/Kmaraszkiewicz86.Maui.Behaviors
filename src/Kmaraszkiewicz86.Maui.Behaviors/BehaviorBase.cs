namespace Kmaraszkiewicz86.Maui.Behaviors
{
    /// <summary>
    /// Provides a base class for behaviors that can be attached to <see cref="BindableObject"/> instances.
    /// </summary>
    /// <remarks>This class manages the association between the behavior and the bindable object, ensuring
    /// that the <see cref="BindingContext"/> is synchronized between them. It handles the attachment and detachment of
    /// the behavior, updating the <see cref="AssociatedObject"/> property accordingly.</remarks>
    /// <typeparam name="T">The type of <see cref="BindableObject"/> to which the behavior can be attached.</typeparam>
    public partial class BehaviorBase<T> : Behavior<T> where T : BindableObject
    {
        /// <summary>
        /// Gets the object that is associated with this instance.
        /// </summary>
        public T? AssociatedObject { get; private set; }

        /// <summary>
        /// Attaches the behavior to the specified bindable object and initializes the binding context.
        /// </summary>
        /// <remarks>This method sets the <see cref="AssociatedObject"/> to the specified bindable object
        /// and synchronizes the binding context. It also subscribes to the <see
        /// cref="BindableObject.BindingContextChanged"/> event to update the binding context when it changes.</remarks>
        /// <param name="bindable">The bindable object to which the behavior is being attached. Cannot be null.</param>
        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged!;
        }

        /// <summary>
        /// Handles the detachment of the behavior from the specified bindable object.
        /// </summary>
        /// <remarks>This method is called when the behavior is being removed from the bindable object. It
        /// unsubscribes from the <see cref="BindingContextChanged"/> event and clears the associated object.</remarks>
        /// <param name="bindable">The bindable object from which the behavior is being detached.</param>
        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged!;
            AssociatedObject = null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject!.BindingContext;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
    }
}