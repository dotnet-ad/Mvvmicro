namespace Mvvmicro
{
	using System.Threading.Tasks;

	public abstract class ViewModelBase : Observable
	{
		#region Constructors

		public ViewModelBase(INavigation navigation)
		{
			this.Navigation = navigation;
			this.GoBackCommand = new RelayCommand(() => this.NavigateBackAsync());
		}

		#endregion

		#region Navigation

		public INavigation Navigation { get; }

		public RelayCommand GoBackCommand { get; }

		protected Task NavigateAsync(string url) => this.NavigateAsync(new NavigationUrl(url));

		protected Task NavigateAsync(NavigationUrl url) => this.Navigation.NavigateAsync(url);

		protected Task NavigateBackAsync() => this.Navigation.NavigateBackAsync();

		#endregion
	}
}