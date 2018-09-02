using System;

using UIKit;

namespace Mvvmicro.Sample.Views.iOS
{
	public partial class DayViewController : UIViewController
	{
		public DayViewController(IntPtr ptr) : base(ptr)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

