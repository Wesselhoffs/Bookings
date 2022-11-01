using Bookings.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bookings.Data
{
    public interface IDataProvider
    {
        int GetAmountOfTables();
        int GetOpenHours();

        void TestSerialize(Dictionary<DateOnly, Restaurant_Day> bookings);

        Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookingsAsync();
        Task LogExceptions(string ex);
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
        internal DateTime GetOpeningTime()
        {
            return new DateTime(01, 01, 01, 14, 0, 0); // Implement external config file loader here instead of static nr.
        }


        public async Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookingsAsync()
        {
            await Task.Delay(0);
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

        public async Task LogExceptions(string ex)
        {
            try
            {
                await using (StreamWriter sw = new("Errorlog.txt", true))
                {
                    DateTime dt = new();
                    dt = DateTime.Now;
                    await sw.WriteLineAsync(dt + "\n" + ex + "\n" + "------------------\n");
                }
            }
            catch (Exception)
            {
            }
        }
        public void TestSerialize(Dictionary<DateOnly, Restaurant_Day> bookings)
        {
            var bookedDays = bookings.Values.Where(v => v.ContainsBooking == true).ToList();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Restaurant_Day));
            using (StreamWriter sw = new("Test.xml"))
            {
                foreach (var day in bookedDays)
                {
                    xmlSerializer.Serialize(sw, day);
                }
            }
        }
    }
}
