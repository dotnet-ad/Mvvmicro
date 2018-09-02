namespace Mvvmicro
{
    using System;

    /// <summary>
    /// A container for managing dependencies between objects.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Register a factory for the given type.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="factory">Factory.</param>
        /// <typeparam name="T">The registered type.</typeparam>
        void Register<T>(Func<IContainer, T> factory, bool isInstance = false);

        /// <summary>
        /// Get an instance of the given type.
        /// </summary>
        /// <remarks>
        /// The type should be registered before use.
        /// </remarks>
        /// <returns>The of.</returns>
        /// <typeparam name="T">The requested type.</typeparam>
        T Get<T>();

        /// <summary>
        /// Returns true if the type has been registered.
        /// </summary>
        /// <returns><c>true</c>, if type has been registered, <c>false</c> otherwise.</returns>
        /// <typeparam name="T">The type parameter.</typeparam>
        bool IsRegistered<T>();

        /// <summary>
        /// Create a new instance (even if the type has been registered as an instance).
        /// </summary>
        /// <returns>The new.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        T New<T>();
    }
}
