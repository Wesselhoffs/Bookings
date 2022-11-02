using Bookings.Data;
using Bookings.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bookings.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly IDataProvider BookingsDataProvider;
        private DateTime selectedCalendarDate;
        private Restaurant_Day selectedRestaurantDay;
        private HoursOpen selectedHourOpen;
        private Table selectedTable;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<HoursOpen> HoursOpen { get; } = new();
        public ObservableCollection<Table> Tables { get; } = new();
        public ObservableCollection<Restaurant_Day> RestaurantDay { get; } = new();
        public ObservableCollection<Customer> ActiveBookingsForSelectedDay { get; } = new();
        public ObservableCollection<ImageSource> TableBackground { get; } = new();
        public Dictionary<DateOnly, Restaurant_Day> BookingsCalendar { get; set; }

        public DateTime SelectedCalendarDate
        {
            get => selectedCalendarDate;
            set
            {
                selectedCalendarDate = value;
                SetSelectedRestaurantDay();
                DisplayHoursOpenForSelectedDay();
                DisplayActiveBookings();
                RaisePropertyChanged();
            }
        }

        public Restaurant_Day SelectedRestaurantDay
        {
            get => selectedRestaurantDay;
            set
            {
                selectedRestaurantDay = value;
                UpdateTableBackgrounds();
                RaisePropertyChanged();
            }
        }
        public HoursOpen SelectedHourOpen
        {
            get => selectedHourOpen;
            set
            {
                selectedHourOpen = value;
                DisplayTablesForSelectedHourOpen();
                UpdateTableBackgrounds();
                RaisePropertyChanged();
            }
        }
        public Table SelectedTable
        {
            get => selectedTable;
            set
            {
                selectedTable = value;
                RaisePropertyChanged();
            }
        }

        public void UpdateTableBackgrounds()
        {
            int counter = 0;
            var fourSeatTables = from table in Tables
                                 where table.TotalChairs < 5
                                 select table;
            var eightSeatTables = from table in Tables
                                  where table.TotalChairs > 4
                                  select table;
            try
            {
                if (!TableBackground.Any())
                {
                    int tablesAmount = BookingsDataProvider.GetAmountOfTables();
                    for (int i = 0; i < tablesAmount; i++)
                    {
                        if (i <= 7)
                        {
                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("../../../Images/Table4.png", UriKind.Relative));
                            TableBackground.Add(brush.ImageSource);
                        }
                        else
                        {
                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("../../../Images/Table8.png", UriKind.Relative));
                            TableBackground.Add(brush.ImageSource);
                        }
                    }
                }
                else if (TableBackground.Any() && SelectedHourOpen == null)
                {
                    int tablesAmount = BookingsDataProvider.GetAmountOfTables();
                    for (int i = 0; i < tablesAmount; i++)
                    {
                        if (i <= 7)
                        {
                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("../../../Images/Table4.png", UriKind.Relative));
                            TableBackground[i] = brush.ImageSource;
                        }
                        else
                        {
                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("../../../Images/Table8.png", UriKind.Relative));
                            TableBackground[i] = brush.ImageSource;
                        }
                    }
                }
                else if (SelectedHourOpen != null && Tables.Any())
                {
                    foreach (var table in fourSeatTables)
                    {
                        var brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(new Uri("../../../Images/Table4_" + table.FreeChairs + ".png", UriKind.Relative));
                        TableBackground[counter] = brush.ImageSource;
                        counter++;
                    }
                    foreach (var table in eightSeatTables)
                    {
                        var brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(new Uri("../../../Images/Table8_" + table.FreeChairs + ".png", UriKind.Relative));
                        TableBackground[counter] = brush.ImageSource;
                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                BookingsDataProvider.LogExceptions(ex.ToString());
            }
        }


        public UserViewModel(IDataProvider bookingsDataProvider)
        {
            this.BookingsDataProvider = bookingsDataProvider;
        }

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SetSelectedRestaurantDay()
        {
            DateOnly searchDate = DateOnly.FromDateTime(SelectedCalendarDate);
            BookingsCalendar.TryGetValue(searchDate, out Restaurant_Day? day);
            SelectedRestaurantDay = day;
            if (RestaurantDay.Any() || day == null)
            {
                RestaurantDay.Clear();
                HoursOpen.Clear();
                Tables.Clear();
            }
            if (day != null)
            {
                RestaurantDay.Add(day);
            }
        }
        private void DisplayHoursOpenForSelectedDay()
        {
            if (RestaurantDay.Any())
            {
                RestaurantDay.Clear();
                HoursOpen.Clear();
                Tables.Clear();
            }
            if (SelectedRestaurantDay != null)
            {
                foreach (var hourOpen in SelectedRestaurantDay.Timeslots)
                {
                    HoursOpen.Add(hourOpen);
                }
            }
        }
        private void DisplayTablesForSelectedHourOpen()
        {
            if (Tables.Any() || SelectedHourOpen == null)
            {
                Tables.Clear();
            }
            if (SelectedHourOpen != null)
            {
                foreach (var table in SelectedHourOpen.Tables)
                {
                    Tables.Add(table);
                }
            }
        }
        public void DisplayActiveBookings()
        {
            if (ActiveBookingsForSelectedDay.Any() || SelectedRestaurantDay == null)
            {
                ActiveBookingsForSelectedDay.Clear();
            }
            if (SelectedRestaurantDay != null)
            {
                var bookedCustomers = from hoursOpen in SelectedRestaurantDay.Timeslots
                                      from table in hoursOpen.Tables
                                      from customers in table.BookedCustomer
                                      select customers;

                foreach (var customer in bookedCustomers)
                {
                    if (customer != null)
                    {
                        ActiveBookingsForSelectedDay.Add(customer);
                    }
                }
            }
        }
        public void SerializeThis()
        {
            BookingsDataProvider.SaveBookingsAsync(BookingsCalendar);
        }

        public async Task LoadBookingCalendarAsync()
        {
            BookingsCalendar = await BookingsDataProvider.LoadBookingsAsync();
        }

        internal void Deserialize()
        {
        }
    }
}
