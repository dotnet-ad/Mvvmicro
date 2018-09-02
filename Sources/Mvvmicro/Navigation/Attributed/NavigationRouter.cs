namespace Mvvmicro
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;

	public abstract class NavigationRouter : INavigationRouter
	{
		#region Constructors

		public NavigationRouter()
		{
			this.RegisterRoutes();
		}

		#endregion

		#region Fields

		private List<INavigationRoute> routes = new List<INavigationRoute>();

		private INavigationRoute defaultRoute;

		#endregion

		#region Route registration

		private void RegisterRoutes()
		{
			var methods = this.GetType().GetRuntimeMethods();

			foreach (var m in methods)
			{
				var routeAttribute = m.GetCustomAttributes()
				                      .FirstOrDefault(x => x is RouteAttribute || x is DefaultRouteAttribute);
				if (routeAttribute != null)
				{
					if(routeAttribute is RouteAttribute)
					{
						this.RegisterRoute(new MethodNavigationRoute(((RouteAttribute)routeAttribute).Url, this, m));
					}
					else if (routeAttribute is DefaultRouteAttribute)
					{
						this.defaultRoute = new MethodNavigationRoute(null, this, m);
					}
				}
			}
		}

		public void RegisterRoute(INavigationRoute route) => this.routes.Add(route);

		#endregion

		#region Navigation

		public abstract bool CanNavigateBack { get; }

		public bool CanNavigateTo(NavigationUrl url) => this.routes.Any(x => x.CanExecute(url));

		public abstract Task NavigateBackAsync();

		public Task NavigateToAsync(NavigationUrl url)
		{
			var route = this.routes.FirstOrDefault(x => x.CanExecute(url)) ?? this.defaultRoute;

			if (route == null)
				throw new InvalidOperationException($"No route found for {url}");

			return route.Execute(url);
		}

		#endregion
	}
}
