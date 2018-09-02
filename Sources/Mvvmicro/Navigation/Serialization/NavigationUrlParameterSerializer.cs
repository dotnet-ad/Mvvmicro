namespace Mvvmicro
{
	using System;

	public class NavigationUrlParameterSerializer : INavigationUrlParameterSerializer
    {
		#region Argument serialization

		public string Serialize(object arg) => Convert.ToString(arg);

		public object Deserialize(string arg, Type t) => Convert.ChangeType(arg, t);

		#endregion
	}
}
