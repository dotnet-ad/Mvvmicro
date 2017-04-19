namespace Mvvmicro.Sample.Models
{
	using System;

	public class Forecast
	{
		public string Identifier { get; set; }

		public Location Location { get; set; }

		public Wind wind { get; set; }

		public DateTime Date { get; set; }

		public Condition Condition { get; set; }

		public int Humidity { get; set; }

		public int Pressure { get; set; }

		public int MaxTemperature { get; set; }

		public int MinTemperature { get; set; }

	}
}
