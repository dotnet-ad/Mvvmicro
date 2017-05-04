namespace Mvvmicro
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// A segment part of an url (ie:'Main' or `Detail?arg=4&other=5' in '/Main/Detail?arg=4&other=5').
	/// </summary>
	public class NavigationUrlSegment
	{
		public NavigationUrlSegment(string segment)
		{
			var splits = segment.Split(new char[] { '?' }, System.StringSplitOptions.RemoveEmptyEntries);

			this.Query = new NavigationUrlQuery(splits.ElementAtOrDefault(1));

			var value = splits.ElementAtOrDefault(0)?.Trim();

			if (IsTemplateValue(value))
			{
				this.IsTemplate = true;
				value = value.Substring(1, value.Length - 2);
			}

			this.Value = value;
		}

		public static bool IsTemplateValue(string value) => value != null && value.StartsWith("{") && value.EndsWith("}");

		#region Fields

		private INavigationUrlParameterSerializer serializer = new NavigationUrlParameterSerializer();

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Mvvmicro.NavigationUrlSegment"/> is template (ie:'{index}' in `/home/{index}?arg=4&other=5' )
		/// </summary>
		/// <value><c>true</c> if is template; otherwise, <c>false</c>.</value>
		public bool IsTemplate { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Mvvmicro.NavigationUrlSegment"/> is wildcard (ie:'*' in `/*/detail?arg=4&other=5' ).
		/// </summary>
		/// <value><c>true</c> if is wildcard; otherwise, <c>false</c>.</value>
		public bool IsWildcard => this.Value == "*";

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Mvvmicro.NavigationUrlSegment"/> is wildcard (ie:'*' in `/*/detail?arg=4&other=5' ).
		/// </summary>
		/// <value><c>true</c> if is wildcard; otherwise, <c>false</c>.</value>
		public bool IsDoubleWildcard => this.Value == "**";

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
		public bool Match(NavigationUrlSegment segment) => this.IsDoubleWildcard || this.IsWildcard || this.IsTemplate || this.Value == segment?.Value;

		#endregion

		#region Templated path argument value

		/// <summary>
		/// Try to get the value of the argument with the given key is templated
		/// </summary>
		/// <returns>The get.</returns>
		/// <param name="t">T.</param>
		/// <param name="key">Key.</param>
		public bool TryGet(NavigationUrlSegment segment, Type t, out object value, [CallerMemberName] string key = null)
		{
			if(this.IsTemplate && this.Value == key)
			{
				value = serializer.Deserialize(segment.Value, t);
				return true;
			}

			value = null;
			return false;
		}

		#endregion

		public override string ToString() => $"{this.Value}{this.Query?.ToString()}";
	}
}
