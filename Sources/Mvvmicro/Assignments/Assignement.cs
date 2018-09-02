namespace Mvvmicro
{
	using System.Collections.Generic;

	/// <summary>
	/// The result of an assignment of an Observable property.
	/// </summary>
	public class Assignement<T>
	{
		public Assignement(Observable owner, string property, T oldValue, T newValue)
		{
			this.Owner = owner;
			this.PropertyName = property;
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.HasChanged = !EqualityComparer<T>.Default.Equals(oldValue, newValue);
		}

		/// <summary>
		/// Gets the Obserbable object that has the updated property.
		/// </summary>
		/// <value>The owner.</value>
		public Observable Owner { get; }

		/// <summary>
		/// Gets the name of the property that has been assigned.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName { get; }

		/// <summary>
		/// Gets a value indicating whether the new value is different from the old one.
		/// </summary>
		/// <value><c>true</c> if has changed; otherwise, <c>false</c>.</value>
		public bool HasChanged { get; }

		/// <summary>
		/// Gets the old value.
		/// </summary>
		/// <value>The old value.</value>
		public T OldValue { get; }

		/// <summary>
		/// Gets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public T NewValue { get; }

		/// <summary>
		/// If the value has changed, then raise a set of other property.
		/// </summary>
		/// <returns>The raise.</returns>
		/// <param name="properties">Properties.</param>
		public Assignement<T> ThenRaise(params string[] properties)
		{
			if(this.HasChanged)
			{
				this.Owner.RaiseProperties(properties);
			}

			return this;
		}

		/// <summary>
		/// If the value has changed, then raise a set of commands.
		/// </summary>
		/// <returns>The raise can execute changed.</returns>
		/// <param name="commands">Commands.</param>
		public Assignement<T> ThenRaiseCanExecuteChanged(params IRelayCommand[] commands)
		{
			if (this.HasChanged)
			{
				foreach (var c in commands)
				{
					c.RaiseCanExecuteChanged();
				}
			}

			return this;
		}

	}
}
