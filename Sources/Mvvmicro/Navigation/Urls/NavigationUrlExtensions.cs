namespace Mvvmicro
{
	using System;
	using System.Linq;

    public static class NavigationUrlExtensions
    {
		/// <summary>
		/// Indicates whether the given url matches the current first url segments.
		/// </summary>
		/// <returns><c>true</c>, if with was startsed, <c>false</c> otherwise.</returns>
		/// <param name="url">URL.</param>
		public static bool StartsWith(this NavigationUrl @this, NavigationUrl url)
		{
			if (@this.Segments.Count() >= url.Segments.Count())
			{
				var thisSegments = @this.Segments.Take(url.Segments.Count()).Select(x => x.Value.Trim());
				var urlSegments = url.Segments.Select(x => x.Value.Trim());
				return thisSegments.SequenceEqual(urlSegments);
			}

			return false;
		}

		public static NavigationUrl TrimStart(this NavigationUrl @this, NavigationUrl url)
		{
			if (@this.StartsWith(url))
			{
				var segments = @this.Segments.Skip(url.Segments.Count()).ToArray();
				return new NavigationUrl(segments);
			}

			return url;
		}

		/// <summary>
		/// Indicates whether the given url matches the current first url segments.
		/// </summary>
		/// <returns><c>true</c>, if with was startsed, <c>false</c> otherwise.</returns>
		/// <param name="url">URL.</param>
		public static bool StartsWith(this NavigationUrl @this, string url) => @this.StartsWith(new NavigationUrl(url));

		public static NavigationUrl TrimStart(this NavigationUrl @this, string url) => @this.TrimStart(new NavigationUrl(url));
    }
}
