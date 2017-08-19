namespace Mvvmicro
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    /// <summary>
    /// Subscribes to Observable property changes according to a viable application lifecycle.
    /// </summary>
    public class NotifyPropertyObserver<TObserver, TObservable>
        where TObservable : INotifyPropertyChanged
        where TObserver : class
    {
        #region Constructors

        public NotifyPropertyObserver(TObservable observable, TObserver observer)
        {
            this.observable = observable;
            this.observable.PropertyChanged += OnPropertyChanged;
            this.observer = new WeakReference<TObserver>(observer);
        }

        #endregion

        #region Fields

        private bool hasBeenActive;

        private TObservable observable;

        private WeakReference<TObserver> observer;

        private Dictionary<string, Action> propertyObservers = new Dictionary<string, Action>();

        private HashSet<string> pendingChanges = new HashSet<string>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether all property observers should be triggered at first registration (default: true).
        /// </summary>
        /// <value><c>true</c> if should trigger initial values; otherwise, <c>false</c>.</value>
        public bool ShouldTriggerInitialValues { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether all changes (while inactive) should be queued and triggers when becoming active again.
        /// </summary>
        /// <value><c>true</c> if should trigger pending changes; otherwise, <c>false</c>.</value>
        public bool ShouldTriggerPendingChanges { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Mvvmicro.NotifyPropertyObserver`2"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Observe the specified property.
        /// </summary>
        /// <returns>The observe.</returns>
        /// <param name="property">Property.</param>
        /// <param name="whenChanged">When changed.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public NotifyPropertyObserver<TObserver, TObservable> Observe<T>(Expression<Func<TObservable, T>> property, Action<TObservable,T> whenChanged)
        {
            if (this.IsActive)
                throw new InvalidOperationException("Property observers can only be configured before activation.");

            var expression = (MemberExpression)property.Body;
            var propertyName = expression.Member.Name;
            var getter = property.Compile();
            Action action = () =>
            {
                var newValue = getter(this.observable);
                whenChanged(this.observable,newValue);
            };
            this.propertyObservers[propertyName] = action;

            return this;
        }

        /// <summary>
        /// Start the property observers, and triggers pending changes first.
        /// </summary>
        public void Start()
        {
            if (!this.IsActive)
            {
                if (!this.hasBeenActive && this.ShouldTriggerInitialValues)
                {
                    foreach (var property in this.propertyObservers)
                    {
                        property.Value();
                    }
                }

                if(this.ShouldTriggerPendingChanges)
                {
                    foreach (var change in this.pendingChanges)
                    {
                        if (this.propertyObservers.TryGetValue(change, out Action action))
                        {
                            action();
                        }
                    } 
                }

                this.pendingChanges = new HashSet<string>();
                this.IsActive = true;
                this.hasBeenActive = true;
            }
        }

        /// <summary>
        /// Stop this property observers.
        /// </summary>
        public void Stop()
        {
            if (this.IsActive)
            {
                this.propertyObservers = new Dictionary<string, Action>();
                this.IsActive = false;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (observer.TryGetTarget(out TObserver _))
            {
                if (this.IsActive)
                {
                    if(this.propertyObservers.TryGetValue(e.PropertyName, out Action action))
                    {
                        action();
                    }
                }
                else 
                {
                    this.pendingChanges.Add(e.PropertyName);
                }
            }
            else
            {
                ((TObservable)sender).PropertyChanged -= OnPropertyChanged;
            }
        }

        #endregion
    }
}
