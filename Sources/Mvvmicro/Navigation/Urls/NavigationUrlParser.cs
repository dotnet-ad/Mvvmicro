namespace Mvvmicro
{
    using System;
    using System.Linq;

    /// <summary>
    /// A helper for parsing a given url and extracting each components and query fluently.
    /// </summary>
    public class NavigationUrlParser
    {
        #region Constructors

        private NavigationUrlParser(int currentSegment, bool isSuccess, NavigationUrl url)
        {
            this.currentSegment = currentSegment;
            this.isSuccess = isSuccess;
            this.url = url;
        }

        public NavigationUrlParser(string url) : this(0, true, new NavigationUrl(url))
        {
        }

        #endregion

        #region Fields

        private bool isSuccess;

        private int currentSegment;

        private NavigationUrl url;


        #endregion

        #region Properties

        public bool IsSuccess => (this.currentSegment == this.url.Segments.Length) && this.isSuccess;

        private string CurrentSegment => this.url.Segments.ElementAt(currentSegment).Value;

        #endregion

        public NavigationUrlParser WithSegment(string segment)
        {
            if (this.isSuccess)
            {
                if (this.currentSegment < this.url.Segments.Length)
                {
                    this.isSuccess = (segment == this.CurrentSegment);
                    this.currentSegment++;
                }
                else
                {
                    this.isSuccess = false;
                }
            }

            return new NavigationUrlParser(this.currentSegment, this.isSuccess, this.url);
        }

        public NavigationUrlParser WithDynamicSegment<T>(out T value)
        {
            value = default(T);

            if (this.isSuccess)
            {
                if (this.currentSegment < this.url.Segments.Length)
                {
                    try
                    {
                        var current = this.CurrentSegment;
                        value = (T)Convert.ChangeType(current, typeof(T));
                        this.currentSegment++;
                    }
                    catch (Exception)
                    {
                        this.isSuccess = false;
                    }
                }
                else
                {
                    this.isSuccess = false;
                }
            }

            return new NavigationUrlParser(this.currentSegment, this.isSuccess, this.url);
        }

        public NavigationUrlParser WithQuery(Action<NavigationUrlQueryParser> arguments)
        {
            if (this.isSuccess)
            {
                var query = new NavigationUrlQueryParser(this.url.Segments.Last().Query);
                arguments(query);
                this.isSuccess = query.IsSuccess;
            }

            return new NavigationUrlParser(this.currentSegment, this.isSuccess, this.url);

        }
    }
}
