namespace Mvvmicro.Extensions
{
	using System;
	using System.Text;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// Extensions for helping with formating of url query strings.
	/// </summary>
	public static class QueryExtensions
	{
		/// <summary>
		/// Creates a query string from query paramaters.
		/// </summary>
		/// <returns>The query string.</returns>
		/// <param name="parameters">Parameters.</param>
		public static string ToQueryString(this Dictionary<string, string> parameters)
		{
			if (parameters == null || parameters.Count == 0)
				return "";

			var result = new StringBuilder("?");

			foreach (var item in parameters)
			{
				if(result.Length > 1) result.Append("&");
				result.Append(Uri.EscapeDataString(item.Key));
				result.Append("=");
				result.Append(Uri.EscapeDataString(item.Value));
			}

			return result.ToString();
		}

		/// <summary>
		/// Extracts the query parameters from a query string.
		/// </summary>
		/// <returns>The query parameters.</returns>
		/// <param name="queryString">Query string.</param>
		public static Dictionary<string, string> ToQueryParameters(this string queryString)
		{
			var result = new Dictionary<string, string>();
			if (string.IsNullOrEmpty(queryString))
				return result;
			
			var pairs = queryString.TrimStart('?').Split('&');

			foreach (var pair in pairs)
			{
				var splits = pair.Split('=');
				var key = Uri.UnescapeDataString(splits.ElementAtOrDefault(0));
				var value = Uri.UnescapeDataString(splits.ElementAtOrDefault(1));
				result.Add(key,value);
			}

			return result;
		}
	}
}
