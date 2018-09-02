namespace Mvvmicro.Sample.ViewModels
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Mvvmicro.Sample.Models;
	using System.Linq;

	public class DayViewModel : ViewModelBase
	{
		public DayViewModel(IWeatherApi api, IDatabase database)
		{
			this.api = api;
			this.database = database;
			this.UpdateCommand = new AsyncRelayCommand<string>(ExecuteUpdateCommand);
		}

		#region Fields

		private double longitude, latitude;

		#endregion

		#region Injected services

		private readonly IDatabase database;

		private readonly IWeatherApi api;

		private DayForecast model;

		private IEnumerable<HourItemViewModel> forecast = new HourItemViewModel[0];

		#endregion

		private DayForecast Model
		{
			get { return this.model; }
			set
			{ 
				this.Set(ref this.model, value)
				    .ThenRaise(nameof(Name), 
			                   nameof(Condition), 
			                   nameof(MaxTemperature), 
			                   nameof(MinTemperature),
							   nameof(Humidity),
							   nameof(Forecast)); 
			}
		}

		public string Name => model?.Date.ToString("dddd");

		public Condition Condition => model?.Condition ?? Condition.Unknown;

		public int MaxTemperature => model?.MaxTemperature ?? 0;

		public int MinTemperature => model?.MinTemperature ?? 0;

		public int Humidity => model?.Humidity ?? 0;

		public IEnumerable<HourItemViewModel> Forecast => model?.Hours?.Select(x => new HourItemViewModel(x));

		#region Commands

		public AsyncRelayCommand<string> UpdateCommand { get; }

		private async Task ExecuteUpdateCommand(string identifier, System.Threading.CancellationToken token)
		{
			this.Model = await this.database.GetAsync(identifier);
		}

		#endregion
	}
}
