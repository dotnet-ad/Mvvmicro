namespace Mvvmicro
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using System.Linq;

	public class Observable : INotifyPropertyChanged
	{
		public Observable()
		{
			
		}


		#region Set and raise bindable property value

		protected Assignement<T> Set<T>(ref T field, T value, [CallerMemberName]string name = null)
		{
			var result = new Assignement<T>(this,name,field, value);

			if (result.HasChanged)
			{
				field = result.NewValue;
				RaiseProperty(name);
			}

			return result;
		}

		public void RaiseProperty(string property) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

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
}
