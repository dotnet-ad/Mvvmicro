namespace Mvvmicro
{
	using System;

    public interface INavigationUrlParameterSerializer
    {
		#region Argument serialization

		/// <summary>
		/// Serialize the specified arg into a string.
		/// </summary>
		/// <returns>The serialize.</returns>
		/// <param name="arg">Argument.</param>
		string Serialize(object arg);

		/// <summary>
		/// Deserialize the specified arg from string to the given type.
		/// </summary>
		/// <returns>The deserialize.</returns>
		/// <param name="arg">Argument.</param>
		/// <param name="t">T.</param>
		object Deserialize(string arg, Type t);

		#endregion
	}
}
