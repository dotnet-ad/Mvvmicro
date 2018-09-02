using System.Reflection;
namespace Mvvmicro
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Mvvmicro.Extensions;

	/// <summary>
	/// A query part of an url segment (ie:'?arg=4&other=5' in '/Main/Detail?arg=4&other=5').
	/// </summary>
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
				this.parameters[pair.Key] = this.serializer.Serialize(pair.Value);
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

		private INavigationUrlParameterSerializer serializer = new NavigationUrlParameterSerializer();

		private Dictionary<string, string> parameters = new Dictionary<string, string>();

		#endregion

		#region Query arguments

		/// <summary>
		/// Set the value of the argument with the given key.
		/// </summary>
		/// <returns>The set.</returns>
		/// <param name="value">Value.</param>
		/// <param name="key">Key.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Set<T>(T value, [CallerMemberName] string key = null) => parameters[key] = serializer.Serialize(value);

		/// <summary>
		/// Get the value of the argument with the given key.
		/// </summary>
		/// <returns>The get.</returns>
		/// <param name="key">Key.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Get<T>([CallerMemberName] string key = null) => (T)this.Get(typeof(T), key);

		/// <summary>
		/// Get the value of the argument with the given key.
		/// </summary>
		/// <returns>The get.</returns>
		/// <param name="t">T.</param>
		/// <param name="key">Key.</param>
		public object Get(Type t, [CallerMemberName] string key = null)
		{
			string stringValue;
			if(parameters.TryGetValue(key, out stringValue))
			{
				var value = serializer.Deserialize(stringValue, t);
				return value;
			}

			if (t.GetTypeInfo().IsValueType)
			{
				return Activator.CreateInstance(t);
			}

			return null;
		}

		#endregion

		public override string ToString() => this.String;
	}
}
