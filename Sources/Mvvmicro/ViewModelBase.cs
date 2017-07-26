namespace Mvvmicro
{
	using System;

	/// <summary>
	/// A view model base class that adds navigation elements to the observable class.
	/// </summary>
	public abstract class ViewModelBase : Observable
	{
		#region Navigation

		/// <summary>
		/// Occurs when a navigation is requested.
		/// </summary>
		public event EventHandler<NavigationArgs> NavigationRequested;

		/// <summary>
		/// Navigate the specified query with then given direction.
		/// </summary>
		/// <returns>The navigate.</returns>
		/// <param name="prepareQuery">Prepares the query.</param>
		/// <param name="direction">Direction.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Navigate<T>(Action<NavigationUrlSegment> prepareQuery, NavigationDirection direction = NavigationDirection.Forward) 
		{
			var segment = new NavigationUrlSegment(typeof(T).FullName);
			prepareQuery(segment);
			this.Navigate(new NavigationUrl(new [] { segment }), direction);
		}

		/// <summary>
		/// Navigate to the specified url with the given direction.
		/// </summary>
		/// <returns>The navigate.</returns>
		/// <param name="url">URL.</param>
		/// <param name="direction">Direction.</param>
		public void Navigate(string url, NavigationDirection direction = NavigationDirection.Forward) => this.Navigate(new NavigationUrl(url), direction);

		/// <summary>
		/// Navigate the specified url with the given direction.
		/// </summary>
		/// <returns>The navigate.</returns>
		/// <param name="url">URL.</param>
		/// <param name="direction">Direction.</param>
		public void Navigate(NavigationUrl url, NavigationDirection direction = NavigationDirection.Forward) => this.NavigationRequested?.Invoke(this, new NavigationArgs(direction, url));

		/// <summary>
		/// Navigates the back (with empty url and backward direction).
		/// </summary>
		public void NavigateBack() => this.NavigationRequested?.Invoke(this, new NavigationArgs(NavigationDirection.Backward));

		#endregion
	}
}