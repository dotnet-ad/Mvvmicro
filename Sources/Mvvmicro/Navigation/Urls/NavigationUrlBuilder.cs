using System.Xml.Linq;
using System.Collections.Generic;
namespace Mvvmicro
{
    using System;
    using System.Linq;

    /// <summary>
    /// A helper for parsing a given url and extracting each components and query fluently.
    /// </summary>
    public class NavigationUrlBuilder
    {
        private NavigationUrlBuilder(NavigationUrl url)
        {
            this.Root = url.Segments.FirstOrDefault()?.Value;
            this.url = url;
        }

        public NavigationUrlBuilder(string root)
        {
            this.Root = root;
            this.url = new NavigationUrl(root);
        }

        public String Root { get; }

        private NavigationUrl url;

        public NavigationUrlBuilder WithSegment(string segment)
        {
            var newUrl = new NavigationUrl(url.Segments.Concat(new[] { new NavigationUrlSegment(segment) }).ToArray());
            return new NavigationUrlBuilder(newUrl);
        }

        public NavigationUrl Build(Action<NavigationUrlQuery> updateQuery = null)
        {
            var newQuery = new NavigationUrlQuery(url.Segments.Last().Query);
            var segments = new List<NavigationUrlSegment>();

            for (int i = 0; i < this.url.Segments.Count(); i++)
            {
                var segment = this.url.Segments[i];
                if(i == this.url.Segments.Length - 1)
                {
                    segment = new NavigationUrlSegment(segment.Value, newQuery);
                }
                else
                {
                    segment = new NavigationUrlSegment(segment.Value);
                }

                segments.Add(segment);
            }

            updateQuery?.Invoke(newQuery);
            var newUrl = new NavigationUrl(segments.ToArray());
            return newUrl;
        }
    }
}
