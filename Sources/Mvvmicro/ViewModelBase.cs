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

		private Task NavigateAsync(string url) => this.NavigateAsync(new NavigationUrl(url));

		private Task NavigateAsync(NavigationUrl url) => this.Navigation.NavigateAsync(url);

		private Task NavigateBackAsync() => this.Navigation.NavigateBackAsync();

		#endregion
	}
}