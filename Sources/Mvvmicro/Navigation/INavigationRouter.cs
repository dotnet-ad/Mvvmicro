namespace Mvvmicro
{
	using System.Threading.Tasks;

	/// <summary>
	/// A navigation abstraction for mobile or desktop applications.
	/// </summary>
	public interface INavigationRouter
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Mvvmicro.INavigation"/> can navigate back.
		/// </summary>
		/// <value><c>true</c> if can navigate back; otherwise, <c>false</c>.</value>
		bool CanNavigateBack { get; }

		/// <summary>
		/// Navigates to the specified url.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="url">URL.</param>
		Task NavigateToAsync(NavigationUrl url);

		/// <summary>
		/// Navigates back.
		/// </summary>
		/// <returns>The back async.</returns>
		Task NavigateBackAsync();
	}

	public static class INavigationRouterExtensions
	{
		public static Task NavigateToAsync(this INavigationRouter nav, string url) => nav.NavigateToAsync(new NavigationUrl(url));
	}
}