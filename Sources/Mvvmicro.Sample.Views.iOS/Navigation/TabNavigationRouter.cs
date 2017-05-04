namespace Mvvmicro.Sample.Views.iOS
{
	using UIKit;
	using System.Threading.Tasks;
	using System;
	using System.Collections.Generic;

	public class TabNavigationRouter : NavigationRouter
	{
		public TabNavigationRouter(UITabBarController controller)
		{
			this.controller = controller;
		}

		#region Fields

		private List<Tuple<string, INavigationRouter>> subroutes = new List<Tuple<string, INavigationRouter>>();

		private UITabBarController controller;

		#endregion

		private INavigationRouter CurrentSubRouter => this.subroutes[(int)this.controller.SelectedIndex].Item2;

		public override bool CanNavigateBack => CurrentSubRouter.CanNavigateBack;

		public override Task NavigateBackAsync() => CurrentSubRouter.NavigateBackAsync();

		#region Routes

		[Route("/{tab}")]
		public void SelectTab(string tab) => this.controller.SelectedIndex = this.subroutes.FindIndex(x => tab == x.Item1);

		[Route("/{tab}/**")]
		public Task SelectTab(string tab, [SubRoute]NavigationUrl suburl)
		{
			this.SelectTab(tab);
			return CurrentSubRouter.NavigateToAsync(suburl);
		}

		#endregion
	}
}