namespace Mvvmicro
{
	public abstract class ViewModelBase : Observable
	{
		#region Constructors

		public ViewModelBase(INavigationRouter navigation)
		{
			this.Navigation = navigation;
			this.NavigateBackCommand = new AsyncRelayCommand((c) => this.Navigation.NavigateBackAsync(), () => this.Navigation.CanNavigateBack);
		}

		#endregion

		#region Navigation

		/// <summary>
		/// Gets the navigation router.
		/// </summary>
		/// <value>The navigation.</value>
		public INavigationRouter Navigation { get; }

		/// <summary>
		/// Gets the command to go back through navigation router.
		/// </summary>
		/// <value>The navigate back command.</value>
		public IAsyncRelayCommand NavigateBackCommand { get; }

		#endregion
	}
}