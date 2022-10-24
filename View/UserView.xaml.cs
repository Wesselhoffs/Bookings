using Bookings.Data;
using Bookings.ViewModel;
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
            ViewModel = new UserViewModel(new DataProvider());
            DataContext = ViewModel;
            Loaded += UserView_Loaded;
        }

        private async void UserView_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadBookingCalendarAsync();
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
            MessageBox.Show(ViewModel.SelectedCalendarDate.ToString());
        }
    }
}
