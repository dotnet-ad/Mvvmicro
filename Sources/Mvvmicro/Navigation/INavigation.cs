namespace Mvvmicro
{
	using System.Threading.Tasks;

	/// <summary>
	/// A navigation abstraction for mobile or desktop applications.
	/// </summary>
	public interface INavigation
	{
		/// <summary>
		/// Navigates to the specified url.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="url">URL.</param>
		Task NavigateAsync(NavigationUrl url);

		/// <summary>
		/// Navigates back.
		/// </summary>
		/// <returns>The back async.</returns>
		Task NavigateBackAsync();
	}
}