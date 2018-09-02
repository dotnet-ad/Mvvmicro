namespace Mvvmicro.Sample.Views.iOS
{
    using System;
    using UIKit;
    using Mvvmicro.Sample.ViewModels;

	public partial class HomeViewController : UIViewController
	{
		public HomeViewController(IntPtr ptr) : base(ptr)
		{
            this.ViewModel = new HomeViewModel(null, null);
            this.observer = this.ViewModel.CreateObserver(this);
		}

        public HomeViewModel ViewModel { get; }

        private NotifyPropertyObserver<HomeViewController, HomeViewModel> observer;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.observer
                .Observe(vm => vm.Title, (vm, value) => this.Title = value)
                .Start();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.observer.Stop();
        }
	}
}

