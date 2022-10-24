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
            int openHoursAmount = new DataProvider().GetOpenHours();
            this.date = new DateOnly();
            this.Timeslots = new HoursOpen[openHoursAmount];
            DateTime dateTime = new DateTime(01, 01, 01, 14, 0, 0);
            for (int i = 0; i < openHoursAmount; i++)
            {
                Timeslots[i] = new HoursOpen(dateTime);
                dateTime = dateTime.AddHours(1);
            }
        }
        public Restaurant_Day(DateOnly date)
        {
            int openHoursAmount = new DataProvider().GetOpenHours();
            this.date = date;
            this.Timeslots = new HoursOpen[openHoursAmount];
            DateTime dateTime = new DateTime(01, 01, 01, 14, 0, 0);
            for (int i = 0; i < openHoursAmount; i++)
            {
                Timeslots[i] = new HoursOpen(dateTime);
                dateTime = dateTime.AddHours(1);
            }
        }
    }
}
