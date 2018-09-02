namespace Mvvmicro
{
    using System;

    /// <summary>
    /// A helper for parsing a given url and extracting each components and query fluently.
    /// </summary>
    public class NavigationUrlQueryParser
    {
        public NavigationUrlQueryParser(NavigationUrlQuery query)
        {
            this.isSuccess = true;
            this.query = query;
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

            return this;
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

            return this;
        }

        public bool IsSuccess => this.isSuccess;
    }
}
