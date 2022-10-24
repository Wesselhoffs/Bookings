using Bookings.Data;
using Bookings.ViewModel;
using System.Windows;
using System.Windows.Controls;

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
            Loaded += UserView_Loaded;
        }

        private async void UserView_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadBookingCalendarAsync();
        }

        private void NewBookingButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ViewModel.SelectedCalendarDate.ToString());
        }
    }
}
