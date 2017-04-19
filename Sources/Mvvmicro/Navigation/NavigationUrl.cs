namespace Mvvmicro
{
	using System.Linq;

	/// <summary>
	/// Represents a navigation URL composed of several segments (ie: '/Main/Detail?id=5&title=kjoidsfj').
	/// </summary>
	public class NavigationUrl
	{
		#region Constructor

		public NavigationUrl(string url)
		{
			var segments = url?.Split('/', '\\') ?? new string[0];
			this.Segments = segments.Select(x => new NavigationUrlSegment(x)).ToArray();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the segments.
		/// </summary>
		/// <value>The segments.</value>
		public NavigationUrlSegment[] Segments { get; }

		#endregion
	}
}
