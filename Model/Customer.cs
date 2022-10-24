using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Model
{
    public class Customer
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SpecialRequests { get; set; }
        public int? PhoneNumber { get; set; }

        public Customer()
        {
            this.FirstName = null;
            this.LastName = null;
            this.SpecialRequests = null;
            this.PhoneNumber = null;
        }
        public Customer(string? firstName, string? lastName, string? specialRequests, int? phoneNumber)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.SpecialRequests = specialRequests;
            this.PhoneNumber = phoneNumber;
        }

    }

}
