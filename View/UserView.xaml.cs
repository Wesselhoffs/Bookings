using Bookings.Data;
using Bookings.Model;
using Bookings.ViewModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Bookings.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using System.Text.RegularExpressions;

namespace Bookings.View
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        private readonly UserViewModel ViewModel;
        public UserView()
        {
            InitializeComponent();
            ViewModel = new UserViewModel(new DataProvider());
            DataContext = ViewModel;
            ViewModel.UpdateTableBackgrounds();
            Loaded += UserView_Loaded;
        }

        private async void UserView_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadBookingCalendarAsync();
            DateTime today = DateTime.Now;
            ViewModel.SelectedCalendarDate = today;
            Booking_Calendar.DisplayDateStart = today;
            today = today.AddYears(1);
            Booking_Calendar.DisplayDateEnd = today;
        }
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void NewBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (KitchenLayout.Visibility == Visibility.Visible)
            {
                AddbookingGrid.Visibility = Visibility.Visible;
                KitchenLayout.Visibility = Visibility.Hidden;
            }
            else
            {
                AddbookingGrid.Visibility = Visibility.Hidden;
                KitchenLayout.Visibility = Visibility.Visible;
            }

        }

        private void Table8_9_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBookingButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SerializeThis();
        }

        private void add_booking_button_Click(object sender, RoutedEventArgs e)
        {
            string? firstName, lastName, phonerNr, specReq;
            int chairsNeeded;
            var tempTable = ViewModel.SelectedTable;
            var tempHour = ViewModel.SelectedHourOpen;
            firstName = customerFirstNameTextbox.Text;
            lastName = customerLastNameTextbox.Text;
            phonerNr = customerPhoneNrTextbox.Text;
            specReq = customerSpecReqTextbox.Text;

            Regex housePhone = new(@"\b\d{2,4}-\d{5,6}");
            Regex cellPhone = new(@"^(([+]46)|(0046)|(0))\s*(7[0236])\s*(\d{4})\s*(\d{3})$");

            if (chairsNeeded_combobox.SelectedItem != null && int.TryParse(chairsNeeded_combobox.SelectedItem.ToString(), out chairsNeeded))
            {
                if (chairsNeeded > 0)
                {
                    if (string.IsNullOrWhiteSpace(lastName))
                    {
                        MessageBox.Show("Du måste ange minst ett efternamn till bokningen.", "Efternamn krävs");
                        return;
                    }
                    else if (housePhone.IsMatch(phonerNr) || cellPhone.IsMatch(phonerNr))
                    {
                        DateOnly date = DateOnly.FromDateTime(ViewModel.SelectedCalendarDate);
                        ViewModel.SelectedTable.BookedCustomer.Add(new(date, tempTable, tempHour, firstName, lastName, specReq, phonerNr, chairsNeeded));
                        ViewModel.DisplayActiveBookings();
                        ViewModel.UpdateTableBackgrounds();
                        KitchenLayout.Visibility = Visibility.Visible;
                        AddbookingGrid.Visibility = Visibility.Hidden;
                        ClearAllText();
                    }
                    else
                    {
                        MessageBox.Show($"Du har inte angett ett giltigt telefonnummer.\n+467XXXXXXXX, 00467XXXXXXXX, 07XXXXXXXX\n" +
                                        $"Eller hemnummer inkl. riktnummer.", "Ogiltigt telefonnummer");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Du kan inte boka 0 stolar. Välj minst 1.\nFinns det inte mer, välj en annan dag, eller ett annat bord.", "Fel antal stolar");
                }
            }
            else
            {
                MessageBox.Show("Du har inte angivit antal stolar till bokningen.", "Ange antal platser");
                return;
            }
        }

        private void ClearAllText()
        {
            customerFirstNameTextbox.Clear();
            customerLastNameTextbox.Clear();
            customerPhoneNrTextbox.Clear();
            customerSpecReqTextbox.Clear();
        }

        private void cancel_booking_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mboxResult = MessageBox.Show("Är du säker på att du vill avbryta?\nAll text går förlorad.", "Ångra bokning", MessageBoxButton.YesNo);
            if (mboxResult == MessageBoxResult.Yes)
            {
                KitchenLayout.Visibility = Visibility.Visible;
                AddbookingGrid.Visibility = Visibility.Hidden;
                ClearAllText();
            }
            else
            {
                return;
            }
        }

        private void activeBookingsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("poop");

        }

        private void DeleteBookinButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Deserialize();
        }
    }
}
