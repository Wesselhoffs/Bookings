using Bookings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Model
{
    internal class Restaurant_Day
    {
        public DateOnly date { get; set; }
        public bool ActiveBooking { get; set; }
        public HoursOpen[] Timeslots { get; set; }

        public Restaurant_Day()
        {
            this.date = new DateOnly();
            this.ActiveBooking = false;
            this.Timeslots = new HoursOpen[new DataProvider().GetOpenHours()];
        }
        public Restaurant_Day(HoursOpen[] timeslots)
        {
            this.Timeslots = timeslots;
        }
    }
}
