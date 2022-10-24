namespace Bookings.Model
{
    public class Customer
    {
        public Table? CustomerTable { get; set; }
        public HoursOpen? CustomerBookedhour { get; set; }
        public string? BookingInformation { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SpecialRequests { get; set; }
        public string? PhoneNumber { get; set; }

        public Customer()
        {
            this.CustomerTable = null;
            this.CustomerBookedhour = null;
            this.BookingInformation = null;
            this.FirstName = null;
            this.LastName = null;
            this.SpecialRequests = null;
            this.PhoneNumber = null;
        }
        public Customer(Table table, HoursOpen hour, string? firstName, string? lastName, string? specialRequests, string? phoneNumber)
        {
            this.CustomerTable = table;
            this.CustomerBookedhour = hour;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.SpecialRequests = specialRequests;
            this.PhoneNumber = phoneNumber;
            this.BookingInformation = hour.Time + "\t\t" + firstName + "\r\n" + table.Name + "\t\t\t" + lastName;
        }

    }

}
