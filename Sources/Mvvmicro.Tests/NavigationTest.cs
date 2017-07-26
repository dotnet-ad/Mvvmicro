namespace Mvvmicro.Tests
{
	using NUnit.Framework;


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
		public void Navigation_Building_ValidUrl()
		{
			var url = new NavigationUrl("/Main/Detail");
			url.AddArg("a1", "E R");
			url.AddArg("a2", 5);
			url.AddArg("a3", true);
			const string expected = "/Main/Detail?a1=E%20R&a2=5&a3=True";
			Assert.AreEqual(expected, url.ToString());
		}
	}
}
