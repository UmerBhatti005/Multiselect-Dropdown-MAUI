using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiApp2
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ItemViewModel> Items { get; set; }
        public ObservableCollection<string> SelectedItems { get; set; }

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
            Items = new ObservableCollection<ItemViewModel>
            {
                new ItemViewModel { Name = "Item 1" },
                new ItemViewModel { Name = "Item 2" },
                new ItemViewModel { Name = "Item 3" },
                new ItemViewModel { Name = "Item 4" }
            };
            SelectedItems = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void UpdateSelectedItems()
        {
            SelectedItems.Clear();
            foreach (var selectedItem in Items.Where(i => i.IsSelected))
            {
                SelectedItems.Add(selectedItem.Name);
            }
            OnPropertyChanged(nameof(SelectedItems));
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> SelectedItems { get; set; }
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
        //private void BindDataOnClicked(object sender, EventArgs e)
        //{
        //    MainViewModel mainViewModel = BindingContext as MainViewModel;
        //    mainViewModel.Items = new ObservableCollection<ItemViewModel>
        //    {
        //        new ItemViewModel { Name = "Item 1" },
        //        new ItemViewModel { Name = "Item 2" },
        //        new ItemViewModel { Name = "Item 3" },
        //        new ItemViewModel { Name = "Item 4" }
        //    };
        //    mainViewModel.OnPropertyChanged(nameof(mainViewModel.Items));
        //}
        private void OnSelectItemsClicked(object sender, EventArgs e)
        {
            popupGrid.IsVisible = true;
        }
        private void OnDoneClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as MainViewModel;
            if (viewModel != null)
            {
                UpdateSelectedItems();
            }
            popupGrid.IsVisible = false;
        }
        private void ShowDataOnClicked(object sender, EventArgs e)
        {
            foreach (var item in SelectedItems)
            {
                Console.WriteLine(item);
            }
        }
        private void UpdateSelectedItems()
        {
            MainViewModel mainViewModel = BindingContext as MainViewModel;
            SelectedItemsContainer.Children.Clear();
            foreach (var item in mainViewModel.Items.Where(i => i.IsSelected))
            {
                var frame = new Frame
                {
                    Padding = new Thickness(5),
                    BorderColor = Color.FromHex("#bdc3c7"),
                    BackgroundColor = Color.FromHex("#bdc3c7"),
                    CornerRadius = 20,
                    Margin = new Thickness(3, 2),
                    HasShadow = true,
                    Content = new Label
                    {
                        Text = item.Name,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        TextColor = Color.FromHex("#2c3e50")
                    }
                };
                //SelectedItemsContainer.Children.Add(frame);
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) =>
                {
                    SelectedItemsContainer.Children.Remove(frame);
                    item.IsSelected = false;
                    mainViewModel.OnPropertyChanged(nameof(SelectedItemsContainer.Children));
                };
                frame.GestureRecognizers.Add(tapGesture);

                SelectedItemsContainer.Children.Add(frame);
                mainViewModel.OnPropertyChanged(nameof(SelectedItemsContainer));
            }
        }
    }

    public class Item : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ObservableCollection<string> collection)
            {
                if (collection.Count() > 0)
                {
                    return string.Join(", ", collection);
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
