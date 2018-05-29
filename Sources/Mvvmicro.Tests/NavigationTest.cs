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

        [Test()]
        public void NavigationBuilder_Building_ValidUrl()
        {
            var main = new NavigationUrlBuilder("Main");
            var url = main.WithSegment("Detail").Build((q) => q.Set("E R", "a1").Set(5, "a2").Set(true, "a3"));
            const string expected = "/Main/Detail?a1=E%20R&a2=5&a3=True";
            Assert.AreEqual(expected, url.ToString());

            var other = main.WithSegment("Other");
            url = other.Build();
            const string otherExpected = "/Main/Other";
            Assert.AreEqual(otherExpected, url.ToString());

            url = other.WithSegment("Finally").Build();
            const string finalyExpected = "/Main/Other/Finally";
            Assert.AreEqual(finalyExpected, url.ToString());
        }

        [Test()]
        public void NavigationParser_Parsing_ValidUrl()
        {
            var parser = new NavigationUrlParser("/Main/Detail/03?a1=E%20R&a2=5&a3=True");

            string a1 = null;
            int a2 = 0;
            bool a3 = false;

            Assert.IsTrue(parser.WithSegment("Main")
                                .WithSegment("Detail")
                                .WithDynamicSegment(out int id)
                                .WithQuery((q) => q.WithRequired("a1", out a1).WithRequired("a2", out a2).WithRequired("a3", out a3))
                                .IsSuccess);

            Assert.AreEqual(3, id);
            Assert.AreEqual("E R", a1);
            Assert.AreEqual(5, a2);
            Assert.AreEqual(true, a3);

        }

        [Test()]
        public void NavigationParser_Parsing_InvalidSegment()
        {
            var parser = new NavigationUrlParser("/Main/Detail/03?a1=E%20R&a2=5&a3=True");

            Assert.IsFalse(parser.WithSegment("Main")
                                 .WithSegment("Woops")
                                 .IsSuccess);
        }

        [Test()]
        public void NavigationParser_Parsing_InvalidDynamicSegment()
        {
            var parser = new NavigationUrlParser("/Main/Detail/03?a1=E%20R&a2=5&a3=True");

            Assert.IsFalse(parser.WithSegment("Main")
                                 .WithSegment("Detail")
                                 .WithDynamicSegment(out bool badType)
                                 .IsSuccess);
        }

        [Test()]
        public void NavigationParser_Parsing_MissingRequiredQueryArgument()
        {
            var parser = new NavigationUrlParser("/Main/Detail/03?a1=E%20R&a2=5&a3=True");

            Assert.IsFalse(parser.WithSegment("Main")
                                 .WithSegment("Detail")
                                 .WithDynamicSegment(out int id)
                                 .WithQuery((q) => q.WithRequired("missing", out string missing))
                                 .IsSuccess);
        }

        [Test()]
        public void NavigationParser_Parsing_MissingOptionalQueryArgument()
        {
            var parser = new NavigationUrlParser("/Main/Detail/03?a1=E%20R&a2=5&a3=True");

            Assert.IsTrue(parser.WithSegment("Main")
                                 .WithSegment("Detail")
                                 .WithDynamicSegment(out int id)
                                 .WithQuery((q) => q.WithOptional("missing", (string missing) => { }))
                                 .IsSuccess);
        }
	}
}
