namespace Mvvmicro
{
    using System;

    /// <summary>
    /// A helper for parsing a given url and extracting each components and query fluently.
    /// </summary>
    public class NavigationUrlQueryParser
    {
        private NavigationUrlQueryParser(bool isSuccess, NavigationUrlQuery query)
        {
            this.isSuccess = isSuccess;
            this.query = query;
        }

        public NavigationUrlQueryParser(NavigationUrlQuery query) : this(true,query)
        {
        }

        #region Fields

        private bool isSuccess;

        private NavigationUrlQuery query;

        #endregion

        public NavigationUrlQueryParser WithRequired<T>(string name, out T value)
        {
            value = default(T);

            if (this.isSuccess)
            {
                this.isSuccess = query.TryGet<T>(out value, name);
            }

            return new NavigationUrlQueryParser(this.isSuccess, this.query);
        }

        public NavigationUrlQueryParser WithOptional<T>(string name, Action<T> value)
        {
            if (this.isSuccess)
            {
                if (query.TryGet<T>(out T result, name))
                {
                    value(result);
                }
            }

            return new NavigationUrlQueryParser(this.isSuccess, this.query);
        }

        public bool IsSuccess => this.isSuccess;
    }
}
