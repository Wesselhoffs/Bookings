using System;
using System.Collections.Generic;

namespace Bookings.Model
{

    public class Table
    {
        public bool IsBooked { get; set; }
        public string? Name { get; set; }
        public int? FreeChairs { get; set; }
        public List<Customer> BookedCustomer { get; } = new();
     
        public Table(int chairs)
        {
            this.IsBooked = false;
            this.Name = null;
            this.FreeChairs = chairs;
        }
        public Table(string? name,int chairs)
        {
            this.IsBooked = false;
            this.Name = name;
            this.FreeChairs = chairs;
        }
    }
}
