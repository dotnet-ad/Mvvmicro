namespace Mvvmicro
{
	using System;

	/// <summary>
	/// A view model base class that adds navigation elements to the observable class.
	/// </summary>
    public abstract class ViewModelBase : Observable, IViewModel
	{
		#region Navigation

		public event EventHandler<NavigationArgs> NavigationRequested;

		public void Navigate<T>(Action<NavigationUrlSegment> prepareQuery, NavigationDirection direction = NavigationDirection.Forward) 
		{
			var segment = new NavigationUrlSegment(typeof(T).FullName);
			prepareQuery(segment);
			this.Navigate(new NavigationUrl(new [] { segment }), direction);
		}

		public void Navigate(string url, NavigationDirection direction = NavigationDirection.Forward) => this.Navigate(new NavigationUrl(url), direction);

		public void Navigate(NavigationUrl url, NavigationDirection direction = NavigationDirection.Forward) => this.NavigationRequested?.Invoke(this, new NavigationArgs(direction, url));

		public void NavigateBack() => this.NavigationRequested?.Invoke(this, new NavigationArgs(NavigationDirection.Backward));

		#endregion
	}
}