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
            var backGround = new ImageBrush();
            backGround.ImageSource = new BitmapImage(new Uri("../../../Images/Table", UriKind.Relative));
        }
    }
}
