namespace Mvvmicro.Sample.Models
{
	using System;
	using System.Threading.Tasks;
	using System.Linq;

	public class DemoWeatherApi : IWeatherApi
	{
		private static int uniqueId;

		private static Random random = new Random();

		private static Array conditions = Enum.GetValues(typeof(Condition));

		private static DayForecast CreateForecast(int i, DateTime date, Location location)
		{
			return new DayForecast
			{
				Identifier = $"{uniqueId++}", 
				Date = date + TimeSpan.FromDays(i),
				Condition = (Condition)conditions.GetValue(random.Next(conditions.Length)),
				MinTemperature = random.Next(5, 10),
				MaxTemperature = random.Next(11, 15),
				Location = location,
				Humidity = random.Next(0, 15),
				Pressure = random.Next(1018, 1020),
				wind = new Wind { Direction = random.Next(0, 360), Speed = random.Next(0, 30) }

			};
		}

		public async Task<DayForecast[]> GetForecastAsync(double longitude, double latitude)
		{
			var date = DateTime.Now.Date;
			var location = new Location()
			{
				City = "Gotham",
				Country = "USA",
				Latitude = latitude,
				Longitude = longitude,
			};

			var result = Enumerable.Range(0, 7).Select((_, d) => CreateForecast(d, date, location)).ToArray();

			result.First().Hours = Enumerable.Range(0, 24).Select((_, h) => CreateForecast(0, date + TimeSpan.FromHours(h), location)).ToArray();

			await Task.Delay(2000);

			return result;
		}
	}
}
