namespace Mvvmicro
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Observable : INotifyPropertyChanged
    {
        #region Set and raise bindable property value

        /// <summary>
        /// Raise the PropertyChanged event with the given property name.
        /// </summary>
        /// <param name="property">Property.</param>
        public void RaiseProperty(string property) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        /// <summary>
        /// Raise the PropertyChanged event for all the given property names.
        /// </summary>
        /// <param name="properties">Properties.</param>
        public void RaiseProperties(params string[] properties)
        {
            foreach (var property in properties)
            {
                this.RaiseProperty(property);
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public static class ObservableExtensions
    {
        /// <summary>
        /// Set the value in the given field, only if the two values aren't equal. If the value is changed, then a 
        /// PropertyChanged event is raised with the given property name.
        /// </summary>
        /// <returns>The result of the assignment.</returns>
        /// <param name="field">Field reference.</param>
        /// <param name="value">The new value.</param>
        /// <param name="name">Name of the set property.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Assignement<TObservable, T> Set<TObservable,T>(this TObservable obs, ref T field, T value, [CallerMemberName]string name = null)
            where TObservable : Observable
        {
            var result = new Assignement<TObservable, T>(obs, name, field, value);

            if (result.HasChanged)
            {
                field = result.NewValue;
                obs.RaiseProperty(name);
            }

            return result;
        }
    }
}
