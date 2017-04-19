namespace Mvvmicro.Sample.ViewModels
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Mvvmicro.Sample.Models;
	using System.Linq;

	public class HomeViewModel : ViewModelBase
	{
		public HomeViewModel(IWeatherApi api, IDatabase database, INavigation navigation) : base(navigation)
		{
			this.api = api;
			this.database = database;
			this.UpdateCommand = new AsyncRelayCommand(ExecuteUpdateCommand);
		}

		#region Fields

		private double longitude, latitude;

		private IEnumerable<DayItemViewModel> forecast = new DayItemViewModel[0];

		#endregion

		#region Injected services

		private readonly IDatabase database;

		private readonly IWeatherApi api;

		#endregion

		#region Bindable properties

		public double Longitude
		{
			get { return this.longitude; }
			set { this.Set(ref this.longitude, value);}
		}

		public double Latitude
		{
			get { return this.longitude; }
			set { this.Set(ref this.longitude, value); }
		}

		public IEnumerable<DayItemViewModel> Forecast
		{
			get { return this.forecast; }
			set { this.Set(ref this.forecast, value); }
		}

		#endregion

		#region Commands

		public AsyncRelayCommand UpdateCommand { get; }

		private async Task ExecuteUpdateCommand(CancellationToken token)
		{
			var models = await this.api.GetForecastAsync(this.Longitude, this.Latitude);
			if(models != null)
			{
				await this.database.InsertOrUpdateAsync(models);
				this.Forecast = models.Select(x => new DayItemViewModel(x));
			}
		}

		#endregion
	}
}
