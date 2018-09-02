namespace Mvvmicro
{
	public interface IRelayCommand : System.Windows.Input.ICommand
	{
		void RaiseCanExecuteChanged();

		bool TryExecute(object parameter);
	}
}
