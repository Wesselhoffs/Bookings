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

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<HoursOpen> HoursOpen { get; } = new();
        public ObservableCollection<Table> Tables { get; } = new();
        public ObservableCollection<Restaurant_Day> RestaurantDay { get; } = new();

        public Dictionary<DateOnly, Restaurant_Day> BookingsCalendar { get; set; }

        public DateTime SelectedCalendarDate
        {
            get => selectedCalendarDate;
            set
            {
                selectedCalendarDate = value;
                DisplaySelectedRestaurantDayHours();
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
                DisplaySelectedHourOpenTables();
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
        private void DisplaySelectedHourOpenTables()
        {
            if (Tables.Any() || selectedHourOpen == null)
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

        private void DisplaySelectedRestaurantDayHours()
        {
            DateOnly searchDate = DateOnly.FromDateTime(SelectedCalendarDate);
            BookingsCalendar.TryGetValue(searchDate, out Restaurant_Day day);
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
                foreach (var hourOpen in day.Timeslots)
                {
                    HoursOpen.Add(hourOpen);
                }
            }
        }

        public async Task LoadBookingCalendarAsync()
        {
            BookingsCalendar = await BookingsDataProvider.LoadBookingsAsync();
        }
    }
}
