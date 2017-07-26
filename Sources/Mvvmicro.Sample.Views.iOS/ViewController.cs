using System;
using Mvvmicro.Sample.ViewModels;
using UIKit;

namespace Mvvmicro.Sample.Views.iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}


		private HomeViewModel ViewModel;

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			this.ViewModel.NavigationRequested += OnNavigation;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewDidAppear(animated);
			this.ViewModel.NavigationRequested -= OnNavigation;
		}

		private void OnNavigation(object sender, NavigationArgs nav)
		{
			this.PerformSegue(nav.Segment.Value, this);
		}
	}
}
