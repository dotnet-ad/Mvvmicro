namespace Mvvmicro
{
	public interface IRelayCommand : System.Windows.Input.ICommand
	{
		void RaiseCanExecuteChanged();

		void TryExecute(object parameter);
	}
}
