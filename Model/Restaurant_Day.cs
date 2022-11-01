using Bookings.Data;
using System;

namespace Bookings.Model
{
    public class Restaurant_Day
    {
        public DateOnly date { get; set; }
        public HoursOpen[] Timeslots { get; set; }

        public Restaurant_Day()
        {
        }
        public Restaurant_Day(DateOnly date)
        {
            int openHoursAmount = new DataProvider().GetOpenHours();
            this.date = date;
            this.Timeslots = new HoursOpen[openHoursAmount];
            DateTime dateTime = new DataProvider().GetOpeningTime();
            for (int i = 0; i < openHoursAmount; i++)
            {
                Timeslots[i] = new HoursOpen(dateTime);
                dateTime = dateTime.AddHours(1);
            }
        }
    }
}
