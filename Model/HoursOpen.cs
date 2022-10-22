using Bookings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Model
{
    internal class HoursOpen
    {
        public Table[] Tables { get; set; }

        public HoursOpen()
        {
            this.Tables = new Table[new DataProvider().GetAmountOfTables()];
        }
        public HoursOpen(Table[] tables)
        {
            this.Tables = tables;
        }
    }
}
