namespace Mvvmicro.Sample.Models
{
	public class DayForecast : Forecast
	{
		public Forecast[] Hours { get; set; } = new Forecast[0];
	}
}
