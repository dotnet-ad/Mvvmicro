using NUnit.Framework;
using System;
namespace Mvvmicro.Tests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void Navigation_Parsing_ValidUrl()
		{
			var url = new NavigationUrl("/Main/Detail?a1=ER&d2=3");
			Assert.AreEqual(2,url.Segments.Length);
			Assert.AreEqual("Main", url.Segments[0].Value);
			Assert.AreEqual("Detail", url.Segments[1].Value);
			Assert.AreEqual("ER", url.Segments[1].Query.Get<string>("a1"));
			Assert.AreEqual(3, url.Segments[1].Query.Get<int>("d2"));
		}
	}
}
