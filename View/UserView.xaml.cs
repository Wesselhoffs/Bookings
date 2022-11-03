﻿using Bookings.Data;
using Bookings.Model;
using Bookings.ViewModel;
using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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
            ViewModel = new UserViewModel(new DataProvider(GetFilePath()));
            ViewModel.UpdateTableBackgrounds();
            DataContext = ViewModel;
            Loaded += UserView_Loaded;
        }

        private string? GetFilePath()
        {
            try
            {

                MessageBoxResult mResult = MessageBox.Show("Vill du öppna en egen databasfil?", "Ladda Databas", MessageBoxButton.YesNo);
                if (mResult == MessageBoxResult.Yes)
                {
                    OpenFileDialog fDialog = new OpenFileDialog();
                    fDialog.InitialDirectory = Environment.CurrentDirectory;
                    fDialog.FileName = "BookingsDatabase";
                    fDialog.DefaultExt = ".json";
                    fDialog.Filter = "Json files (.json)|*.json";
                    fDialog.ShowDialog();

                    return fDialog.FileName;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Något gick fel");
                return null;
            }
        }

        private async void UserView_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadBookingCalendarAsync();
            DateTime today = DateTime.Now;
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
            AddbookingGrid.Visibility = Visibility.Visible;
            KitchenLayout.Visibility = Visibility.Hidden;
            NewBookingButton.Visibility = Visibility.Hidden;
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
                        NewBookingButton.Visibility = Visibility.Visible;
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
                NewBookingButton.Visibility = Visibility.Visible;
                ClearAllText();
            }
        }

        private void activeBookingsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (activeBookingsView.SelectedItem != null)
            {
                var selectedCustomer = (Customer)activeBookingsView.SelectedItem;
                ShowCustomerInformation(selectedCustomer, "Bokningsinformation", MessageBoxButton.OK);
            }
        }

        private static MessageBoxResult ShowCustomerInformation(Customer selectedCustomer, string header, MessageBoxButton mBoxButtons)
        {
            MessageBoxResult mBoxResult = MessageBox.Show($"Kund & Bokningsinformation\n" +
                            $"---------------\n\n" +
                            $"Datum:\t\t{selectedCustomer.BookedDate}\n" +
                            $"Namn:\t\t{selectedCustomer.FirstName} {selectedCustomer.LastName}\n" +
                            $"Telefonnummer:\t{selectedCustomer.PhoneNumber}\n" +
                            $"Tid:\t\t{selectedCustomer.CustomerBookedhour.Time}\n" +
                            $"Bord:\t\t{selectedCustomer.CustomerTable.Name}\n" +
                            $"Bokade stolar:\t{selectedCustomer.ChairsNeeded}\n" +
                            $"Önskemål:\t{selectedCustomer.SpecialRequests}", header, mBoxButtons);
            return mBoxResult;
        }

        private void DeleteBookinButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeBookingsView.SelectedItem != null)
            {
                var selectedCustomer = (Customer)activeBookingsView.SelectedItem;
                MessageBoxResult mBoxResult = ShowCustomerInformation(selectedCustomer, "VILL DU TA BORT BOKNINGEN", MessageBoxButton.YesNo);
                if (mBoxResult == MessageBoxResult.Yes)
                {
                    selectedCustomer.CustomerTable.FreeChairs += selectedCustomer.ChairsNeeded;
                    int index = selectedCustomer.CustomerTable.BookedCustomer.FindIndex(c => c.BookingInformation.Equals(selectedCustomer.BookingInformation));
                    selectedCustomer.CustomerTable.BookedCustomer.RemoveAt(index);
                    selectedCustomer = null;
                    GC.Collect();
                    ViewModel.DisplayActiveBookings();
                    ViewModel.UpdateTableBackgrounds();
                }
            }
            else
            {
                MessageBox.Show("Du har inte valt någon bokning i fönstret \"Bokningar för valt datum\"\nMarkera en bokning för att ta bort den", "Välj en bokning");
            }
        }
    }
}
