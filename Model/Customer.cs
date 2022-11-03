using System;
using System.Text.Json.Serialization;

namespace Bookings.Model
{
    public class Customer
    {
        [JsonIgnore]
        public Table? CustomerTable { get; set; }
        [JsonIgnore]
        public HoursOpen? CustomerBookedhour { get; set; }
        public string BookedDate { get; set; }
        public string? BookingInformation { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SpecialRequests { get; set; }
        public string? PhoneNumber { get; set; }
        public int? ChairsNeeded { get; set; }


        public Customer()
        {
            this.CustomerTable = null;
            this.CustomerBookedhour = null;
            this.BookingInformation = null;
            this.FirstName = null;
            this.LastName = null;
            this.SpecialRequests = null;
            this.PhoneNumber = null;
            this.ChairsNeeded = null;
        }
        public Customer(DateOnly bookedDate, Table table, HoursOpen hour, string? firstName, string? lastName, string? specialRequests, string? phoneNumber, int chairsNeeded)
        {
            this.BookedDate = bookedDate.ToString();
            this.CustomerTable = table;
            CustomerTable.FreeChairs -= chairsNeeded;
            this.CustomerBookedhour = hour;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.SpecialRequests = specialRequests;
            this.PhoneNumber = phoneNumber;
            this.ChairsNeeded = chairsNeeded;
            if (table.Name == "Bord 10")
            {
                this.BookingInformation = hour.Time + "\t\t" + firstName + "\r\n" + table.Name + ", " + chairsNeeded + " platser\t" + lastName;
            }
            else
            {
                this.BookingInformation = hour.Time + "\t\t" + firstName + "\r\n" + table.Name + ", " + chairsNeeded + " platser\t" + lastName;
            }
        }

    }

}
