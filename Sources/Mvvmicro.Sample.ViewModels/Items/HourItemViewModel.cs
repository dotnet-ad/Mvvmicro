namespace Mvvmicro.Sample.ViewModels
{
	using Mvvmicro.Sample.Models;

	public class HourItemViewModel : DayItemViewModel
	{
		public HourItemViewModel(Forecast model) : base(model)
		{
			this.Name = model.Date.ToString("h");
		}
	}
}
