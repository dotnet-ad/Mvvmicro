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
			var segments = url?.Split(new char[] { '/', '\\' }, System.StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
			this.Segments = segments.Select(x => new NavigationUrlSegment(x)).ToArray();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the segments.
		/// </summary>
		/// <value>The segments.</value>
		public NavigationUrlSegment[] Segments { get; private set; }

		#endregion

		#region Query arguments

		/// <summary>
		/// Adds the argument to the query of the last segment.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public NavigationUrl AddArg<T>(string name, T value)
		{
			var segment = this.Segments.LastOrDefault();

			if (segment == null)
			{
				segment = new NavigationUrlSegment("");
				this.Segments = new[] { segment };
			}

			segment.AddArg(name, value);

			return this;
		}

		#endregion

		#region Matches

		/// <summary>
		/// Indicates whether the given url matches the current url segments.
		/// </summary>
		/// <returns>The match.</returns>
		/// <param name="url">Url.</param>
		public bool Match(string url) => this.Match(new NavigationUrl(url));

		/// <summary>
		/// Indicates whether the given url matches the current url segments.
		/// </summary>
		/// <returns>The match.</returns>
		/// <param name="url">Url.</param>
		public bool Match(NavigationUrl url)
		{
			var thisSegments = this.Segments.Select(x => x.Value.Trim());
			var urlSegments = url.Segments.Select(x => x.Value.Trim());
			return thisSegments.SequenceEqual(urlSegments);
		}

		#endregion

		public override string ToString() => $"/{string.Join("/", this.Segments.Select(x => x.ToString()))}";
	}
}
