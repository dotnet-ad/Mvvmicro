namespace Mvvmicro
{
    using System;

	/// <summary>
	/// A view model base class that adds navigation elements to the observable class.
	/// </summary>
    public abstract class ViewModelBase : Observable, IViewModel
	{
        #region Constructors

        public ViewModelBase()
        {
            this.NavigateCommand = new RelayCommand<NavigationArgs>(Navigate);
        }

        #endregion

        #region Navigation

        public RelayCommand<NavigationArgs> NavigateCommand { get; set; }

        public event EventHandler<NavigationArgs> NavigationRequested;

		public void Navigate<T>(Action<NavigationUrlSegment> prepareQuery, NavigationDirection direction = NavigationDirection.Forward) 
		{
			var segment = new NavigationUrlSegment(typeof(T).FullName);
			prepareQuery(segment);
			this.Navigate(new NavigationUrl(new [] { segment }), direction);
		}

        public void Navigate(NavigationArgs args) => this.NavigateCommand?.TryExecute(args);

        public void Navigate(string url, NavigationDirection direction = NavigationDirection.Forward) => this.Navigate(new NavigationArgs(direction,  new NavigationUrl(url)));

		public void Navigate(NavigationUrl url, NavigationDirection direction = NavigationDirection.Forward) => this.Navigate(new NavigationArgs(direction, url));

        public void NavigateBack() => this.Navigate(new NavigationArgs(NavigationDirection.Backward));

        #endregion
    }
}