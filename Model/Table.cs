using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Model
{

    public class Table
    {
        public bool IsBooked { get; set; }
        public string? Name { get; set; }
        public Customer? BookedCustomer { get; set; }
        
        public Table()
        {
            this.IsBooked = false;
            this.Name = null;
            this.BookedCustomer = null;
        }
        public Table(string? name)
        {
            this.IsBooked = false;
            this.Name = name;
            this.BookedCustomer = null;
        }
    }
}
