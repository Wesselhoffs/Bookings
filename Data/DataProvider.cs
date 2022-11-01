using Bookings.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bookings.Data
{
    public interface IDataProvider
    {
        int GetAmountOfTables();
        int GetOpenHours();

        Task TestSerialize(Dictionary<DateOnly, Restaurant_Day> bookings);
        Task DeSerializeCustomers();

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
        public async Task TestSerialize(Dictionary<DateOnly, Restaurant_Day> bookings)
        {
            var customers = from day in bookings.Values
                            from hours in day.Timeslots
                            from timeslot in hours.Tables
                            from customerlist in timeslot.BookedCustomer
                            where customerlist != null
                            select customerlist;
            var myList = customers.ToList();


            using (FileStream fs = File.Create("testfile.json"))
            {
                await JsonSerializer.SerializeAsync(fs, myList);
                await fs.DisposeAsync();
            }
            await DeSerializeCustomers();
        }

        public async Task DeSerializeCustomers()
        {
            using (FileStream fs = File.OpenRead("testfile.json"))
            {
                var list = await JsonSerializer.DeserializeAsync<List<Customer>>(fs);
            }
        }
    }
}
