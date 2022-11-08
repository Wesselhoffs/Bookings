using Bookings.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookings.Data
{
    public interface IDataProvider
    {
        public string SavefilePath { get; set; }
        int GetAmountOfTables();
        int GetOpenHours();
        int GetAmountOfChairsPerHour();

        Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookingsAsync();
        Task SaveBookingsAsync(Dictionary<DateOnly, Restaurant_Day> bookings);
        Task LogExceptions(string ex);
    }
}
