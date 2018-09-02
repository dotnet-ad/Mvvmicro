namespace Mvvmicro.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture()]
    public class ObservableTests
    {
        [Test()]
        public void ThenRaise()
        {
            var changed = new List<string>();
            var vm = new HomeViewModel();
            vm.PropertyChanged += (sender, e) => { changed.Add(e.PropertyName); };

            vm.Firstname = "firstname";
            Assert.IsTrue(changed.SequenceEqual(new[] { nameof(HomeViewModel.Firstname), nameof(HomeViewModel.Fullname), }));
        }
    }
}
