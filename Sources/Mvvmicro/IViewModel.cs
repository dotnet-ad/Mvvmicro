using System;

namespace Mvvmicro
{
    public interface IViewModel
    {
        #region Navigation

        /// <summary>
        /// Occurs when a navigation is requested.
        /// </summary>
        event EventHandler<NavigationArgs> NavigationRequested;

        /// <summary>
        /// Navigate the specified query with then given direction.
        /// </summary>
        /// <returns>The navigate.</returns>
        /// <param name="prepareQuery">Prepares the query.</param>
        /// <param name="direction">Direction.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        void Navigate<T>(Action<NavigationUrlSegment> prepareQuery, NavigationDirection direction = NavigationDirection.Forward);

        /// <summary>
        /// Navigate to the specified url with the given direction.
        /// </summary>
        /// <returns>The navigate.</returns>
        /// <param name="url">URL.</param>
        /// <param name="direction">Direction.</param>
        void Navigate(string url, NavigationDirection direction = NavigationDirection.Forward);

        /// <summary>
        /// Navigate the specified url with the given direction.
        /// </summary>
        /// <returns>The navigate.</returns>
        /// <param name="url">URL.</param>
        /// <param name="direction">Direction.</param>
        void Navigate(NavigationUrl url, NavigationDirection direction = NavigationDirection.Forward);

        /// <summary>
        /// Navigates the back (with empty url and backward direction).
        /// </summary>
        void NavigateBack();

        #endregion
    }
}
