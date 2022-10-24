using Bookings.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Data
{
    public interface IDataProvider
    {
        int GetAmountOfTables();
        int GetOpenHours();

        Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookingsAsync();
        Task<IEnumerable<KeyValuePair<DateOnly, Restaurant_Day>>> SaveBookingsAsync();
    }
    public class DataProvider : IDataProvider
    {     
        public int GetAmountOfTables()
        {
            return 10;      // Implement external config file loader here instead of static nr.
        }

        public int GetOpenHours()
        {            
            return 10;      // Implement external config file loader here instead of static nr.
        }

        
        public async Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookingsAsync()
        {
            await Task.Delay(1000);
            Dictionary<DateOnly, Restaurant_Day> bookings = new();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            for (DateOnly date = DateOnly.FromDateTime(DateTime.Now); date < today.AddYears(1);)
            {
                bookings.Add(date, new Restaurant_Day(date));
                date = date.AddDays(1);
            }
            return bookings;            
        }

        public async Task<IEnumerable<KeyValuePair<DateOnly, Restaurant_Day>>> SaveBookingsAsync()
        {
            throw new NotImplementedException();
        }
    }   
}
