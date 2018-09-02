namespace Mvvmicro
{
	using System.Linq;

	/// <summary>
	/// Represents a navigation URL composed of several segments (ie: '/Main/Detail?id=5&title=kjoidsfj').
	/// </summary>
	public class NavigationUrl
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Mvvmicro.NavigationUrl"/> class.
		/// </summary>
		/// <param name="url">URL.</param>
		public NavigationUrl(string url)
		{
			var segments = url?.Split(new char[] { '/', '\\' }, System.StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
			this.Segments = segments.Select(x => new NavigationUrlSegment(x)).ToArray();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Mvvmicro.NavigationUrl"/> class.
		/// </summary>
		/// <param name="segments">Segments.</param>
		public NavigationUrl(NavigationUrlSegment[] segments)
		{
			this.Segments = segments.ToArray();
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

		public override string ToString() => $"/{string.Join("/", this.Segments.Select(x => x.ToString()))}";
	}
}
