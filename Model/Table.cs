using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bookings.Model
{

    public class Table : INotifyPropertyChanged
    {
        private int? freeChairs;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string? Name { get; set; }
        public int? TotalChairs { get; set; }
        public int? FreeChairs
        {
            get => freeChairs;
            set
            {
                freeChairs = value;
                UpdateBookableChairs();
                RaisePropertyChanged();
            }
        }
        public List<Customer> BookedCustomer { get; } = new();
        public ObservableCollection<int?> BookableChairs { get; } = new();

        public Table(int chairs)
        {
            this.Name = null;
            this.TotalChairs = chairs;
            this.FreeChairs = chairs;
            UpdateBookableChairs();
        }
        public Table(string? name, int chairs)
        {
            this.Name = name;
            this.TotalChairs = chairs;
            this.FreeChairs = chairs;
            UpdateBookableChairs();
        }

        private void UpdateBookableChairs()
        {
            if (BookableChairs.Any())
            {
                BookableChairs.Clear();
            }            
            for (int? i = this.FreeChairs; i >= 0; i--)
            {
                BookableChairs.Add(i);
            }
        }
        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
