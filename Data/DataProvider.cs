using Bookings.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Data
{
    interface IDataProvider
    {
        int GetAmountOfTables();
        int GetOpenHours();

        Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookingsAsync();
        Task<Dictionary<DateOnly, Restaurant_Day>> SaveBookingsAsync();
    }
    internal class DataProvider : IDataProvider
    {     
        public int GetAmountOfTables()
        {
            return 10;
        }

        public int GetOpenHours()
        {            
            return 10;
        }

        public async Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookingsAsync()
        {
            await Task.Delay(1000);
            return new Dictionary<DateOnly, Restaurant_Day>();
        }

        public async Task<Dictionary<DateOnly, Restaurant_Day>> SaveBookingsAsync()
        {
            throw new NotImplementedException();
        }
    }   
}
