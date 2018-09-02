namespace Mvvmicro.Sample.Models
{
	using System.Linq;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class MemoryDatabase : IDatabase
	{
		private List<DayForecast> items = new List<DayForecast>();

		public Task InsertOrUpdateAsync(IEnumerable<DayForecast> items)
		{
			if(items != null)
			{
				this.items.RemoveAll(x => items.Any(i => i.Identifier == x.Identifier));
				this.items.AddRange(items);
			}

			return Task.FromResult(true);
		}

		public Task<DayForecast> GetAsync(string id) => Task.FromResult(this.items.FirstOrDefault(x => x.Identifier == id));
	}
}
