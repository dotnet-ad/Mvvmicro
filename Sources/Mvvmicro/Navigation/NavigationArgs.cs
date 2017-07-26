namespace Mvvmicro
{
	using System;
	using System.Linq;

	public class NavigationArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Mvvmicro.NavigationArgs"/> class.
		/// </summary>
		/// <param name="direction">Direction.</param>
		/// <param name="url">URL.</param>
		public NavigationArgs(NavigationDirection direction = NavigationDirection.Forward, NavigationUrl url = null)
		{
			this.Url = url;
			this.Direction = direction;
		}

		/// <summary>
		/// Gets the navigation direction.
		/// </summary>
		/// <value>The direction.</value>
		public NavigationDirection Direction { get; }

		/// <summary>
		/// Gets the requested URL.
		/// </summary>
		/// <value>The URL.</value>
		public NavigationUrl Url { get; }

		/// <summary>
		/// Gets the root segment.
		/// </summary>
		/// <value>The segment.</value>
		public NavigationUrlSegment Segment => Url?.Segments.FirstOrDefault();

		/// <summary>
		/// Gets the child URL (original url without the current segment).
		/// </summary>
		/// <value>The child URL.</value>
		public NavigationUrl ChildUrl => new NavigationUrl(Url?.Segments.Skip(1).ToArray());
	}
}
