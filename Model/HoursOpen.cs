using Bookings.Data;
using System;

namespace Bookings.Model
{
    public class HoursOpen
    {
        public string Time { get; set; }
        public Table[] Tables { get; set; }

        public HoursOpen(DateTime time)
        {
            int tablesAmount = new DataProvider().GetAmountOfTables();
            this.Tables = new Table[tablesAmount];
            for (int i = 0; i < tablesAmount; i++)
            {
                if (i <= 7)
                {
                    Tables[i] = new Table("Bord " + (i + 1), 4);
                }
                else
                {
                    Tables[i] = new Table("Bord " + (i + 1), 8);

                }
            }
            Time = time.ToString("HH:mm") + " - " + time.AddHours(1).ToString("HH:mm");
        }
    }
}
