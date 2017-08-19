namespace Mvvmicro.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture()]
    public class ObserverTests
    {
        HomeViewModel viewmodel;
        HomeView view;
        NotifyPropertyObserver<HomeView, HomeViewModel> observer;

        [SetUp]
        public void Initialize()
        {
            this.viewmodel = new HomeViewModel();
            this.view = new HomeView();
            this.observer = viewmodel.CreateObserver(view);
        }

        [Test()]
        public void NotStarted_ValueChanged_NotTrigger()
        {
            observer.Observe(vm => vm.Firstname, (vm, value) => view.Title = value);

            viewmodel.Firstname = "firstname";

            Assert.AreNotEqual(viewmodel.Firstname, view.Title);
        }

        [Test()]
        public void Started_InitialValue_InitialTrigger()
        {
            observer.Observe(vm => vm.Firstname, (vm, value) => view.Title = value);

            viewmodel.Firstname = "firstname";

            Assert.AreNotEqual(viewmodel.Firstname, view.Title);

            observer.Start();

            Assert.AreEqual(viewmodel.Firstname, view.Title);
        }


        [Test()]
        public void Started_ValueChanged_ValueUpdatedInstantly()
        {
            observer.Observe(vm => vm.Firstname, (vm, value) => view.Title = value).Start();

            viewmodel.Firstname = "firstname";

            Assert.AreEqual(viewmodel.Firstname, view.Title);
        }


        [Test()]
        public void Restarted_PendingValueChanged_ValueApplied()
        {
            observer.Observe(vm => vm.Firstname, (vm, value) => view.Title = value).Start();

            viewmodel.Firstname = "firstname";

            observer.Stop();

            viewmodel.Firstname = "newfirstname";

            Assert.AreNotEqual(viewmodel.Firstname, view.Title);

            observer.Observe(vm => vm.Firstname, (vm, value) => view.Title = value).Start();

            Assert.AreEqual(viewmodel.Firstname, view.Title);
        }
    }
}
