namespace Mvvmicro.Sample.Models
{
	using System.Threading.Tasks;

	public interface IWeatherApi
	{
		Task<DayForecast[]> GetForecastAsync(double longitude, double latitude);
	}
}
