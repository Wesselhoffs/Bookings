using Bookings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Model
{
    public class HoursOpen
    {
        public Table[] Tables { get; set; }

        public HoursOpen()
        {
            int tablesAmount = new DataProvider().GetAmountOfTables();
            this.Tables = new Table[tablesAmount];
            for (int i = 0; i < tablesAmount; i++)
            {
                Tables[i] = new Table();
            }
        }
        public HoursOpen(Table[] tables)
        {
            this.Tables = tables;
        }
    }
}
