namespace Mvvmicro
{
	using System;
	using System.Linq;
	using System.Runtime.CompilerServices;

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

		/// <summary>
		/// Gets the query of the last segment.
		/// </summary>
		/// <value>The last query.</value>
		public NavigationUrlQuery LastQuery => this.Segments.LastOrDefault()?.Query;

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

		#region Templated path argument value

		/// <summary>
		/// Try to get the value of the argument with the given key is templated
		/// </summary>
		/// <returns>The get.</returns>
		/// <param name="t">T.</param>
		/// <param name="key">Key.</param>
		public bool TryGetFromPath(NavigationUrl testedUrl, Type t, out object value, [CallerMemberName] string key = null)
		{
			for (int i = 0; i < testedUrl.Segments.Length; i++)
			{
				var thisSegment = this.Segments[i];
				var testedSegment = testedUrl.Segments[i];

				if(thisSegment.TryGet(testedSegment,t,out value, key))
				{
					return true;
				}
			}

			value = null;
			return false;
		}

		public bool TryGetFromPath<T>(NavigationUrl testedUrl, out T value, [CallerMemberName] string key = null)
		{
			object v;
			if (TryGetFromPath(testedUrl, typeof(T), out v, key))
			{
				value = (T)v;
				return true;
			}

			value = default(T);
			return false;
		}

		/// <summary>
		/// Tries the get the sub URL at the end of the given url.
		/// </summary>
		/// <returns><c>true</c>, if get sub URL was tryed, <c>false</c> otherwise.</returns>
		/// <param name="testedUrl">Tested URL.</param>
		/// <param name="suburl">Suburl.</param>
		public bool TryGetSubUrl(NavigationUrl testedUrl, out NavigationUrl suburl)
		{
			if (testedUrl.Segments.Length >= this.Segments.Length && this.Match(testedUrl) && (this.Segments.LastOrDefault()?.IsDoubleWildcard ?? false))
			{
				suburl = new NavigationUrl(testedUrl.Segments.Skip(this.Segments.Length - 1).ToArray());
				return true;
			}

			suburl = null;
			return false;
		}

		#endregion

		#region Matches

		/// <summary>
		/// Indicates whether the given url matches the current url segments.
		/// </summary>
		/// <returns>The match.</returns>
		/// <param name="url">Url.</param>
		public bool Match(NavigationUrl url)
		{
			var isDoubleWildcard = this.Segments.LastOrDefault()?.IsDoubleWildcard ?? false;

			if (isDoubleWildcard && this.Segments.Length > url.Segments.Length)
				return false;

			if (!isDoubleWildcard && Segments.Length != url.Segments.Length)
				return false;

			for (int i = 0; i < this.Segments.Length; i++)
				{
					var thisSegment = this.Segments[i];
					var urlSegment = url.Segments[i];
					if (!thisSegment.Match(urlSegment))
						return false;
				}

			return true;
		}

		/// <summary>
		/// Indicates whether the given url matches the current url segments.
		/// </summary>
		/// <returns>The match.</returns>
		/// <param name="url">Url.</param>
		public bool Match(string url) => this.Match(new NavigationUrl(url));

		#endregion

		public override string ToString() => $"/{string.Join("/", this.Segments.Select(x => x.ToString()))}";
	}
}
