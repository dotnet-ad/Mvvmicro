namespace Mvvmicro.Sample.ViewModels
{
	using Mvvmicro.Sample.Models;

	public class DayItemViewModel
	{
		public DayItemViewModel(Forecast model)
		{
			this.Identifier = model.Identifier;
			this.Name = model.Date.ToString("dddd");
			this.Condition = model.Condition;
			this.MinTemperature = model.MinTemperature;
			this.MaxTemperature = model.MaxTemperature;
			this.Humidity = model.Humidity;
		}

		public string Identifier { get; protected set; }

		public string Name { get; protected set; }

		public Condition Condition { get; }

		public int MaxTemperature { get; }

		public int MinTemperature { get; }

		public int Humidity { get; }
	}
}
