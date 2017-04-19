namespace Mvvmicro
{
	using System.Collections.Generic;

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

		public Observable Owner { get; }

		public string PropertyName { get; }

		public bool HasChanged { get; }

		public T OldValue { get; }

		public T NewValue { get; }

		public void ThenRaise(params string[] properties) => this.Owner.RaiseProperties(properties);
	}
}
