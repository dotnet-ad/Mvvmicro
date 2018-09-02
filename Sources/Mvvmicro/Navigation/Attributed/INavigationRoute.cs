namespace Mvvmicro
{
	using System.Threading.Tasks;

    public interface INavigationRoute
    {
		bool CanExecute(NavigationUrl url);

		Task Execute(NavigationUrl url);
    }
}
