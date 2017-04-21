namespace Mvvmicro
{
	public interface IRelayCommand : System.Windows.Input.ICommand
	{
		/// <summary>
		/// Raises the CanExecuteChanged event.
		/// </summary>
		void RaiseCanExecuteChanged();

		/// <summary>
		/// Try to execute the command if CanExecute(parameter) is true.
		/// </summary>
		/// <returns><c>true</c>, if execute was tryed, <c>false</c> otherwise.</returns>
		/// <param name="parameter">Parameter.</param>
		bool TryExecute(object parameter);
	}
}
