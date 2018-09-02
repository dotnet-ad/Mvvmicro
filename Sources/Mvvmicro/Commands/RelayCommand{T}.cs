namespace Mvvmicro
{
	using System;

	/// <summary>
	/// An helper command to create implementations of ICommand with a typed argument.
	/// </summary>
	public class RelayCommand<T> : IRelayCommand
	{
		#region Constructors

		public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute ?? ((o) => true);
		}

		#endregion

		#region Fields

		private Action<T> execute;

		private Func<T, bool> canExecute;

		#endregion

		#region Events

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Methods

		public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

		public bool CanExecute(object parameter) => this.canExecute((T)parameter);

		public void Execute(object parameter) => this.execute((T)parameter);

		public void TryExecute(object parameter)
		{
			if (this.CanExecute(parameter))
				this.Execute(parameter);
		}

		#endregion
	}
}
