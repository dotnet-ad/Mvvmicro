namespace Mvvmicro.Sample.Views.iOS
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public abstract class iOSNavigationRouter : INavigationRouter
	{
		public interface INavigationArgument
		{
			Task Push();

			Task Pop();
		}

		#region Fields

		private Stack<INavigationArgument> stack = new Stack<INavigationArgument>();

		private Dictionary<string, INavigationArgument> patterns = new Dictionary<string, INavigationArgument>();

		#endregion

		public bool CanNavigateBack => stack.Count > 1;

		public void Register(string url, INavigationArgument arg) => patterns[url] = arg;

		public virtual Task NavigateBackAsync() => stack.Pop().Pop();

		public virtual Task NavigateToAsync(NavigationUrl url)
		{
			var nav = patterns.Where(x => url.Match(x.Key)).Select(x => x.Value).FirstOrDefault();

			if(nav != null)
			{
				stack.Push(nav);
				return nav.Push();
			}

			return Task.FromResult(true);
		}
	}
}
	