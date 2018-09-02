namespace Mvvmicro
{
	using System;
	using System.ComponentModel;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// An helper command to create asynchronous implementations of ICommand.
	/// </summary>
	public class AsyncRelayCommand : IAsyncRelayCommand
	{
		#region Fields 

		private readonly Func<CancellationToken, Task> execute;

		private readonly Func<bool> canExecute;

		private DateTime? lastExecution;

		private Task execution;

		private CancellationTokenSource cts;

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler<Exception> ExecutionFailed;

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Properties

		public bool IsExecuting => execution != null;

		public DateTime? LastSuccededExecution => this.lastExecution;

		#endregion

		#region Constructors

		public AsyncRelayCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute ?? (() => true);
		}

		#endregion

		#region Methods

		public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

		private void RaiseIsExecuting()
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExecuting)));
			this.RaiseCanExecuteChanged();
		}

		public async void Execute(object parameter)
		{
			this.cts = new CancellationTokenSource();
			this.execution = execute(cts.Token);
			this.RaiseIsExecuting();

			try
			{
				await this.execution;
				this.lastExecution = DateTime.Now;
				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastSuccededExecution)));
			}
			catch (Exception e)
			{
				this.ExecutionFailed?.Invoke(this, e);
			}
			finally
			{
				this.cts = null;
				this.execution = null;
				this.RaiseIsExecuting();
			}
		}

		public void Cancel() => this.cts?.Cancel();

		public bool CanExecute(object parameter) => !this.IsExecuting && this.canExecute();

		public bool TryExecute(object parameter = null)
		{
			if (this.CanExecute(parameter))
			{
				this.Execute(parameter);
				return true;
			}

			return false;
		}

		#endregion
	}
}
