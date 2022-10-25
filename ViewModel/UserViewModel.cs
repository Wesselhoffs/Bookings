using Bookings.Data;
using Bookings.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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


        public Restaurant_Day? SelectedRestaurantDay
        {
            get => selectedRestaurantDay;
            set
            {
                selectedRestaurantDay = value;
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

        public async Task LoadBookingCalendarAsync()
        {
            BookingsCalendar = await BookingsDataProvider.LoadBookingsAsync();
        }
    }
}
