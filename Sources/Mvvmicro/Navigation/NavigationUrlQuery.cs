namespace Mvvmicro
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Mvvmicro.Extensions;

	public class NavigationUrlQuery
	{
		#region Constructor 

		public NavigationUrlQuery(string queryString)
		{
			this.String = queryString;
		}

		public NavigationUrlQuery(Dictionary<string, object> parameters)
		{
			foreach (var pair in parameters)
			{
				this.parameters[pair.Key] = Serialize(pair.Value);
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the query as an url query string (ie: '?id=5&title=kjoidsfj').
		/// </summary>
		/// <value>The URL parameters.</value>
		public string String
		{
			get { return parameters?.ToQueryString(); }
			set { this.parameters = value?.ToQueryParameters() ?? new Dictionary<string, string>(); }
		}

		#endregion

		#region Fields

		private Dictionary<string, string> parameters = new Dictionary<string, string>();

		#endregion

		#region Argument serialization

		private string Serialize(object arg) => Convert.ToString(arg);

		private object Deserialize(string arg, Type t) => Convert.ChangeType(arg, t);

		#endregion

		/// <summary>
		/// Set the value of the argument with the given key.
		/// </summary>
		/// <returns>The set.</returns>
		/// <param name="value">Value.</param>
		/// <param name="key">Key.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Set<T>(T value, [CallerMemberName] string key = null) => parameters[key] = Serialize(value);

		/// <summary>
		/// Get the value of the argument with the given key.
		/// </summary>
		/// <returns>The get.</returns>
		/// <param name="key">Key.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Get<T>([CallerMemberName] string key = null) => (T)Deserialize(parameters[key], typeof(T));
	}
}
