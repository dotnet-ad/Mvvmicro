namespace Mvvmicro
{
	using System;
	using System.ComponentModel;
	using System.Windows.Input;

	/// <summary>
	/// An asynchronous command with a Task based execution.
	/// </summary>
	public interface IAsyncCommand : ICommand, INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when the execution failed.
		/// </summary>
		event EventHandler<Exception> ExecutionFailed;

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Mvvmicro.IAsyncCommand"/> is executing.
		/// </summary>
		/// <value><c>true</c> if is executing; otherwise, <c>false</c>.</value>
		bool IsExecuting { get; }

		/// <summary>
		/// Gets the last succeded execution date (null if never succeeded).
		/// </summary>
		/// <value>The last succeded execution date, or null if never succeeded.</value>
		DateTime? LastSuccededExecution { get; }

		/// <summary>
		/// Cancel the current execution.
		/// </summary>
		void Cancel();
	}
}
