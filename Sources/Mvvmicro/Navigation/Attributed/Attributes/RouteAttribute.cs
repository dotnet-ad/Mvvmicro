namespace Mvvmicro
{
	using System;

	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class RouteAttribute : Attribute
	{
		public RouteAttribute(string url)
		{
			this.Url = new NavigationUrl(url);
		}

		public NavigationUrl Url { get; }
	}
}
