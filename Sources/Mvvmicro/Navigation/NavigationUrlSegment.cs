namespace Mvvmicro
{
	/// <summary>
	/// On segment part of an url (ie:'Main' or `Detail?arg=4&other=5' in '/Main/Detail?arg=4&other=5').
	/// </summary>
	public class NavigationUrlSegment
	{
		public NavigationUrlSegment(string segment)
		{
			var splits = segment.Split('?');
			this.Value = splits?[0];
			this.Query = new NavigationUrlQuery(this.Value);
		}

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
	}
}
