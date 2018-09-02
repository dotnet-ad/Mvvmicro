namespace Mvvmicro
{
	using System.Linq;

	/// <summary>
	/// A segment part of an url (ie:'Main' or `Detail?arg=4&other=5' in '/Main/Detail?arg=4&other=5').
	/// </summary>
	public class NavigationUrlSegment
	{
		public NavigationUrlSegment(string segment)
		{
			var splits = segment.Split(new char[] { '?' }, System.StringSplitOptions.RemoveEmptyEntries);
			this.Value = splits.ElementAtOrDefault(0);
			this.Query = new NavigationUrlQuery(splits.ElementAtOrDefault(1));
		}

		#region Properties

		/// <summary>
		/// Gets the value (ie:'Detail' in `Detail?arg=4&other=5' )
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; }

		/// <summary>
		/// Gets the query (ie:'?arg=4&other=5' in `Detail?arg=4&other=5' )
		/// </summary>
		/// <value>The query.</value>
		public NavigationUrlQuery Query { get; }

		#endregion

		#region Query arguments

		/// <summary>
		/// Adds the argument to the query of the segment.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void AddArg<T>(string name, T value) => this.Query.Set(value, name);

		#endregion

		#region Matches

		/// <summary>
		/// Indicates whether the given segment name matches the current segment value.
		/// </summary>
		/// <returns>The match.</returns>
		/// <param name="segment">Segment.</param>
		public bool Match(string segment) => this.Value?.Trim() == segment?.Trim();

		#endregion

		public override string ToString() => $"{this.Value}{this.Query?.ToString()}";
	}
}
