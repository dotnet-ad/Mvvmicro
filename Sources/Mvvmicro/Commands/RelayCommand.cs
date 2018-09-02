namespace Mvvmicro
{
	using System;

	/// <summary>
	/// An helper command to create implementations of ICommand.
	/// </summary>
    public class RelayCommand : IRelayCommand
	{
		#region Constructors

		public RelayCommand(Action execute, Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute ?? (() => true);
		}

		#endregion

		#region Fields

		private Action execute;

		private Func<bool> canExecute;

		#endregion

		#region Events

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Methods

		public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

		public bool CanExecute(object parameter) => this.canExecute();

		public void Execute(object parameter) => this.execute();

		#endregion
	}
}
