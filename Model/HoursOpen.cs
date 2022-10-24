﻿using Bookings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Tables[i] = new Table("Bord " + (i + 1));
            }
            Time = time.ToString("HH:mm") + " - " + time.AddHours(1).ToString("HH:mm");
        }
        public HoursOpen(Table[] tables)
        {
            this.Tables = tables;
        }
    }
}