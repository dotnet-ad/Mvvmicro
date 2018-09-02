namespace Mvvmicro.Sample.Models
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IDatabase
	{
		Task InsertOrUpdateAsync(IEnumerable<DayForecast> items);

		Task<DayForecast> GetAsync(string id);
	}
}
