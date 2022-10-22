using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Model
{
    internal class Customer
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? PhoneNumber { get; set; }

        public Customer()
        {
            this.FirstName = null;
            this.LastName = null;
            this.PhoneNumber = null;
        }
        public Customer(string? firstName, string? lastName, int? phoneNumber)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
        }

    }

}
