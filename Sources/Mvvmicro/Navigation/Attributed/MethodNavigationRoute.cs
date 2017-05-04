namespace Mvvmicro
{
	using System.Reflection;
	using System.Linq;
	using System.Threading.Tasks;
	using System;
	using System.Collections.Generic;

	public class MethodNavigationRoute : INavigationRoute
    {
		public NavigationUrl Url { get; }

		public object Owner { get; }

		public MethodInfo MethodInfo { get;  }

		private bool isTask;

		private ParameterInfo subrouteParameter;

		public MethodNavigationRoute(NavigationUrl url, object owner, MethodInfo method)
		{
			this.Owner = owner;
			this.Url = url;
			this.MethodInfo = method;
			this.isTask = method.ReturnType == typeof(Task);
			var parameter = method.GetParameters();
			this.subrouteParameter = parameter.FirstOrDefault(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(SubRouteAttribute)));
		}

		public Task Execute(params object[] arguments)
		{
			if (this.isTask)
				return MethodInfo.Invoke(this.Owner, arguments) as Task;
			
			MethodInfo.Invoke(this.Owner, arguments);
			return Task.FromResult(true);
		}

		public Task Execute(NavigationUrl url)
		{
			var parameters = this.MethodInfo.GetParameters();

			var arguments = new List<object>();

			foreach (var p in parameters)
			{
				object v;
				if (p.ParameterType == typeof(NavigationUrlQuery))
				{
					v = url.LastQuery;
				}
				else if (p.ParameterType == typeof(NavigationUrl))
				{
					NavigationUrl suburl;
					if (p == subrouteParameter && this.Url.TryGetSubUrl(url, out suburl))
					{
						v = suburl;
					}
					else
					{
						v = url;
					}
				}
				else if (!this.Url.TryGetFromPath(url, p.ParameterType, out v, p.Name))
				{
					v = url.LastQuery.Get(p.ParameterType, p.Name);
				}

				arguments.Add(v);
			}

			return this.Execute(arguments.ToArray());
		}

		public bool CanExecute(NavigationUrl url)
		{
			return this.Url.Match(url);
		}
	}
}
