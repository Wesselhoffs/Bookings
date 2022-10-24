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
        public Customer? BookedCustomer { get; set; }
        
        public Table()
        {
            this.IsBooked = false;
            this.BookedCustomer = new();
        }
    }
}
