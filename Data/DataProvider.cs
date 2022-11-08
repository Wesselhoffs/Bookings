using Bookings.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookings.Data
{
    public class DataProvider : IDataProvider
    {
        public string SavefilePath { get; set; }
        public DataProvider()
        {
            SavefilePath = "BookingsDatabase.json";
        }
        public DataProvider(string? savefilePath)
        {
            if (!string.IsNullOrWhiteSpace(savefilePath))
            {
                SavefilePath = savefilePath;
            }
            else
            {
                SavefilePath = "BookingsDatabase.json";
            }
        }

        public int GetAmountOfTables()
        {
            return 10;      // Implement external config file loader here instead of static nr.
        }
        public int GetAmountOfChairsPerHour()
        {
            return 24;      // Implement external config file loader here instead of static nr.
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
            Dictionary<DateOnly, Restaurant_Day> bookings = new();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            for (DateOnly date = DateOnly.FromDateTime(DateTime.Now); date < today.AddYears(1);)
            {
                bookings.Add(date, new Restaurant_Day(date));
                date = date.AddDays(1);
            }
            if (File.Exists(SavefilePath))
            {
                bookings = await LoadBookedCustomersFromFile(bookings);
                GC.Collect();
                return bookings;
            }
            else
            {
                return bookings;
            }
        }

        private async Task<Dictionary<DateOnly, Restaurant_Day>> LoadBookedCustomersFromFile(Dictionary<DateOnly, Restaurant_Day> bookings)
        {
            var alreadyBookedCustomers = await DeSerializeCustomers();
            foreach (var customer in alreadyBookedCustomers)
            {
                DateOnly dateKeyValue = DateOnly.Parse(customer.BookedDate);
                string customerHour, customerTable;
                customerHour = customer.BookingInformation.Substring(0, 13);
                customerTable = customer.BookingInformation.Substring(customer.BookingInformation.IndexOf(Environment.NewLine) + 2, 7).Trim(',');
                bookings.TryGetValue(dateKeyValue, out Restaurant_Day? rDay);

                var hourIndex = Array.FindIndex(rDay.Timeslots, h => h.Time == customerHour);
                var tableIndex = Array.FindIndex(rDay.Timeslots[hourIndex].Tables, t => t.Name == customerTable);

                customer.CustomerBookedhour = rDay.Timeslots[hourIndex];
                customer.CustomerTable = rDay.Timeslots[hourIndex].Tables[tableIndex];
                customer.CustomerTable.FreeChairs -= customer.ChairsNeeded;
                customer.CustomerTable.BookedCustomer.Add(customer);
            }
            return bookings;
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
        public async Task SaveBookingsAsync(Dictionary<DateOnly, Restaurant_Day> bookings)
        {
            var customers = from day in bookings.Values
                            from hours in day.Timeslots
                            from timeslot in hours.Tables
                            from customerlist in timeslot.BookedCustomer
                            where customerlist != null
                            select customerlist;

            var myBookedCustomers = customers.ToList();

            try
            {
                if (File.Exists(SavefilePath))
                {
                    if (File.Exists(SavefilePath.Replace(".json", "_backup.json")))
                    {
                        File.Delete(SavefilePath.Replace(".json", "_backup.json"));
                    }
                    else
                    {
                        File.Copy(SavefilePath, SavefilePath.Replace(".json", "_backup.json"));
                    }
                }
                using (FileStream fs = File.Create(SavefilePath))
                {
                    await JsonSerializer.SerializeAsync(fs, myBookedCustomers);
                    await fs.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                await LogExceptions(ex.ToString());
            }
        }

        private async Task<List<Customer>> DeSerializeCustomers()
        {
            var customers = new List<Customer>();
            try
            {
                using (FileStream fs = File.OpenRead(SavefilePath))
                {
                    customers = await JsonSerializer.DeserializeAsync<List<Customer>>(fs);
                    await fs.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                await LogExceptions(ex.ToString());
            }
            return customers;
        }
    }
}
