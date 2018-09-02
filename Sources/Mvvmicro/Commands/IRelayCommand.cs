namespace Mvvmicro
{
	public interface IRelayCommand : System.Windows.Input.ICommand
	{
		/// <summary>
		/// Raises the CanExecuteChanged event.
		/// </summary>
		void RaiseCanExecuteChanged();
	}
}
