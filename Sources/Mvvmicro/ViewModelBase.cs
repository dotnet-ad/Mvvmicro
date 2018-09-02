namespace Mvvmicro
{
	public abstract class ViewModelBase : Observable
	{
		#region Constructors

		public ViewModelBase(INavigation navigation)
		{
			this.Navigation = navigation;
			this.NavigateBackCommand = new AsyncRelayCommand((c) => this.Navigation.NavigateBackAsync(), () => this.Navigation.CanNavigateBack);
		}

		#endregion

		#region Navigation

		public INavigation Navigation { get; }

		public IAsyncRelayCommand NavigateBackCommand { get; }

		#endregion
	}
}