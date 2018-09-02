namespace Mvvmicro
{
    using System;
    using System.Linq;

    /// <summary>
    /// A helper for parsing a given url and extracting each components and query fluently.
    /// </summary>
    public class NavigationUrlBuilder
    {
        public NavigationUrlBuilder(string root)
        {
            this.Root = root;
            this.url = new NavigationUrl(root);
        }

        public String Root { get; }

        private NavigationUrl url;

        public NavigationUrlBuilder WithSegment(string segment)
        {
            this.url = new NavigationUrl(url.Segments.Concat(new[] { new NavigationUrlSegment(segment) }).ToArray());
            return this;
        }

        public NavigationUrl Build(Action<NavigationUrlQuery> query = null)
        {
            query?.Invoke(this.url.Segments.Last().Query);
            return this.url;
        }
    }
}
