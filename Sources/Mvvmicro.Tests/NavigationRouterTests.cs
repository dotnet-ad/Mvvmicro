namespace Mvvmicro.Tests
{
	using System;
	using System.Threading.Tasks;
	using NUnit.Framework;

	[TestFixture()]
	public class NavigationRouterTest
	{
		private TestRouter router;

		[SetUp]
		public void Setup()
		{
			this.router = new TestRouter();
		}

		public class TestRouter : NavigationRouter
		{
			public string State { get; private set; }

			public override bool CanNavigateBack { get { throw new NotImplementedException(); } }

			public override Task NavigateBackAsync() { throw new NotImplementedException(); }

			[DefaultRoute]
			public void NavigateToDefault() { this.State = "Default"; }

			[Route("/")]
			public void NavigateToRoot() { this.State = "Root"; }

			[Route("/sub/**")]
			public void NavigateToSubroute([SubRoute]NavigationUrl suburl) { this.State = suburl.ToString(); }

			[Route("/detailWithParameters")]
			public void NavigateToDetailWithParameters(int id, string description) { this.State = $"DetailWithParameters,{id},{description}"; }

			[Route("/detailWithQuery")]
			public void NavigateToDetailWithQuery(NavigationUrlQuery query) { this.State = $"DetailWithQuery,{query.Get<int>("id")},{query.Get<string>("description")}"; }

			[Route("/detailWithUrl")]
			public void NavigateToDetailWithUrl(NavigationUrl url) { this.State = $"DetailWithUrl,{url.LastQuery.Get<int>("id")},{url.LastQuery.Get<string>("description")}"; }


			[Route("/detailWithPath/{id}/{description}")]
			public void NavigateToDetailWithPath(int id, string description) { this.State = $"DetailWithPath,{id},{description}"; }

		}

		[Test()]
		public async Task NavigationRouter_NavigateToAsync_Root()
		{
			await this.router.NavigateToAsync("/");
			Assert.AreEqual("Root", this.router.State);
		}

		[Test()]
		public async Task NavigationRouter_NavigateToAsync_ValidParameters()
		{
			await this.router.NavigateToAsync("/detailWithParameters?id=5&description=sampledescription_example");
			Assert.AreEqual("DetailWithParameters,5,sampledescription_example", this.router.State);
		}

		[Test()]
		public async Task NavigationRouter_NavigateToAsync_ValidQuery()
		{
			await this.router.NavigateToAsync("/detailWithQuery?id=5&description=sampledescription_example");
			Assert.AreEqual("DetailWithQuery,5,sampledescription_example", this.router.State);
		}

		[Test()]
		public async Task NavigationRouter_NavigateToAsync_ValidUrl()
		{
			await this.router.NavigateToAsync("/detailWithUrl?id=5&description=sampledescription_example");
			Assert.AreEqual("DetailWithUrl,5,sampledescription_example", this.router.State);
		}

		[Test()]
		public async Task NavigationRouter_NavigateToAsync_ValidPath()
		{
			await this.router.NavigateToAsync("/detailWithPath/5/sampledescription_example");
			Assert.AreEqual("DetailWithPath,5,sampledescription_example", this.router.State);
		}

		[Test()]
		public async Task NavigationRouter_NavigateToAsync_ValidDefault()
		{
			await this.router.NavigateToAsync("/detailWithUrl?id=5&description=sampledescription_example");
			Assert.AreNotEqual("Default", this.router.State);
			await this.router.NavigateToAsync("/notSupportedPath");
			Assert.AreEqual("Default", this.router.State);
		}


		[Test()]
		public async Task NavigationRouter_NavigateToAsync_ValidSubroute()
		{
			await this.router.NavigateToAsync("/sub/5/sampledescription_example");
			Assert.AreEqual("/5/sampledescription_example", this.router.State);
		}
	}
}
