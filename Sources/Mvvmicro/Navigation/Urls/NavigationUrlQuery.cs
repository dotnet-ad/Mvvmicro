namespace Mvvmicro
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Mvvmicro.Extensions;
    using System.Reflection;

    /// <summary>
    /// A query part of an url segment (ie:'?arg=4&other=5' in '/Main/Detail?arg=4&other=5').
    /// </summary>
    public class NavigationUrlQuery
    {
        #region Constructor 

        public NavigationUrlQuery(string queryString)
        {
            this.String = queryString;
        }

        public NavigationUrlQuery(Dictionary<string, object> parameters)
        {
            foreach (var pair in parameters)
            {
                this.parameters[pair.Key] = this.serializer.Serialize(pair.Value);
            }
        }

        public NavigationUrlQuery(NavigationUrlQuery other)
        {
            foreach (var pair in other.parameters)
            {
                this.parameters[pair.Key] = pair.Value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the query as an url query string (ie: '?id=5&title=kjoidsfj').
        /// </summary>
        /// <value>The URL parameters.</value>
        public string String
        {
            get { return parameters?.ToQueryString(); }
            set { this.parameters = value?.ToQueryParameters() ?? new Dictionary<string, string>(); }
        }

        #endregion

        #region Fields

        private INavigationUrlParameterSerializer serializer = new NavigationUrlParameterSerializer();

        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        #endregion

        #region Query arguments

        /// <summary>
        /// Set the value of the argument with the given key.
        /// </summary>
        /// <returns>The set.</returns>
        /// <param name="value">Value.</param>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public NavigationUrlQuery Set<T>(T value, [CallerMemberName] string key = null)
        {
            parameters[key] = serializer.Serialize(value);
            return this;
        }

        /// <summary>
        /// Get the value of the argument with the given key.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Get<T>([CallerMemberName] string key = null) => (T)this.Get(typeof(T), key);

        public object Get(Type t, [CallerMemberName] string key = null)
        {
            if (this.TryGet(t, out object result, key))
                return result;

            return Activator.CreateInstance(t);
        }

        /// <summary>
        /// Try to get the value of the argument with the given key.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="t">T.</param>
        /// <param name="key">Key.</param>
        public bool TryGet<T>(out T result, [CallerMemberName] string key = null)
        {
            if (this.TryGet(typeof(T), out object resultO, key))
            {
                result = (T)resultO;
                return true;
            }

            result = default(T);
            return false;

        }

        /// <summary>
        /// Try to get the value of the argument with the given key.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="key">Key.</param>
        public bool TryGet(Type t, out object result, [CallerMemberName] string key = null)
        {
            string stringValue;
            if (parameters.TryGetValue(key, out stringValue))
            {
                try
                {
                    if (t == typeof(Guid))
                    {
                        result = new Guid(stringValue);
                        return true;
                    }

                    result = serializer.Deserialize(stringValue, t);
                    return true;
                }
                catch (Exception) { }
            }

            result = null;
            return false;
        }

        #endregion

        public override string ToString() => this.String;
    }
}
