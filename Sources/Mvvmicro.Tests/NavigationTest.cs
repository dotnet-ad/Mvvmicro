namespace Mvvmicro.Tests
{
	using NUnit.Framework;
	using System.Threading.Tasks;


	[TestFixture()]
	public class Test
	{
		[Test()]
		public void Navigation_Parsing_ValidUrl()
		{
			var url = new NavigationUrl("/Main/Detail?a1=ER&d2=3&d3=true");
			Assert.AreEqual(2,url.Segments.Length);
			Assert.AreEqual("Main", url.Segments[0].Value);
			Assert.AreEqual("Detail", url.Segments[1].Value);
			Assert.AreEqual("ER", url.Segments[1].Query.Get<string>("a1"));
			Assert.AreEqual(3, url.Segments[1].Query.Get<int>("d2"));
			Assert.AreEqual(true, url.Segments[1].Query.Get<bool>("d3"));
		}

		[Test()]
		public void Navigation_TryGetFromPath_ValidUrl()
		{
			var url = new NavigationUrl("/{t1}/{t2}?a1=ER&d2=3&d3=true");

			var testUrl = new NavigationUrl("/5/7?d3=true");

			int t1;
			Assert.IsTrue(url.TryGetFromPath(testUrl, out t1, "t1"));
			Assert.AreEqual(5, t1);

			string t2;
			Assert.IsTrue(url.TryGetFromPath(testUrl, out t2, "t2"));
			Assert.AreEqual("7", t2);
		}


		[Test()]
		public void Navigation_Building_ValidUrl()
		{
			var url = new NavigationUrl("/Main/Detail");
			url.AddArg("a1", "E R");
			url.AddArg("a2", 5);
			url.AddArg("a3", true);
			const string expected = "/Main/Detail?a1=E%20R&a2=5&a3=True";
			Assert.AreEqual(expected, url.ToString());
		}

		[Test()]
		public void Navigation_Match_ValidResult()
		{
			var url = new NavigationUrl("/Main/Detail?a1=ER&d2=3");
			Assert.IsTrue(url.Match("/Main/Detail/"));
			Assert.IsTrue(url.Match("/Main/Detail"));
			Assert.IsTrue(url.Match("Main/Detail"));
			Assert.IsTrue(url.Match("Main/Detail?a=5&t=6"));
			Assert.IsTrue(url.Match("Main/Detail/"));
			Assert.IsFalse(url.Match("/Main"));
			Assert.IsFalse(url.Match("/Detail"));

			url = new NavigationUrl("/{root}/Detail?a1=ER&d2=3");
			Assert.IsTrue(url.Match("/Main/Detail?a1=ER&d2=3"));
			Assert.IsTrue(url.Match("/Other/Detail?a1=ER&d2=3"));
			Assert.IsTrue(url.Match("/Other?a1=ER/Detail?a1=ER&d2=3"));
		}

		[Test()]
		public void Navigation_StartsWith_ValidResult()
		{
			var url = new NavigationUrl("/Main/Detail?a1=ER&d2=3");
			Assert.IsTrue(url.StartsWith("/Main/Detail/"));
			Assert.IsTrue(url.StartsWith("/Main/Detail"));
			Assert.IsTrue(url.StartsWith("Main/Detail"));
			Assert.IsTrue(url.StartsWith("Main/Detail?a=5&t=6"));
			Assert.IsTrue(url.StartsWith("Main/Detail/"));
			Assert.IsTrue(url.StartsWith("/Main/"));
			Assert.IsTrue(url.StartsWith("/Main"));
			Assert.IsTrue(url.StartsWith("Main/"));
			Assert.IsTrue(url.StartsWith("Main?a=5&t=6"));
			Assert.IsTrue(url.StartsWith("Main"));
			Assert.IsFalse(url.StartsWith("/Detail"));
			Assert.IsFalse(url.StartsWith("Detail"));
			Assert.IsFalse(url.StartsWith("/Other/Detail"));
		}

		[Test()]
		public void Navigation_TrimStart_ValidResult()
		{
			var url = new NavigationUrl("/Main/Detail?a1=ER&d2=3");
			Assert.AreEqual("/Detail?a1=ER&d2=3", url.TrimStart("/Main").ToString());
			Assert.AreEqual("/", url.TrimStart("/Main/Detail").ToString());
			Assert.AreNotEqual("/Detail?a1=ER&d2=3", url.TrimStart("/Detail").ToString());
		}
	}
}
