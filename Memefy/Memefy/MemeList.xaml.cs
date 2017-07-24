using Memefy.Model;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Memefy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MemeList : ContentPage
    {
        MobileServiceClient client;

        public MemeList(List<MemeCaptions> captionsList)
        {
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new MemeListViewModel(captionsList);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            await DisplayAlert("Selected", e.SelectedItem.ToString(), "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }

    class MemeListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MemeCaptions> Items { get; }
        public ObservableCollection<Grouping<string, MemeCaptions>> ItemsGrouped { get; }

        public MemeListViewModel(List<MemeCaptions> items)
        {
            Items = new ObservableCollection<MemeCaptions>(items);

            var sorted = from item in Items
                         orderby item.Caption
                         group item by item.Caption[0].ToString() into itemGroup
                         select new Grouping<string, MemeCaptions>(itemGroup.Key, itemGroup);

            ItemsGrouped = new ObservableCollection<Grouping<string, MemeCaptions>>(sorted);

            RefreshDataCommand = new Command(
                async () => await RefreshData());
        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            //Load Data Here
            await Task.Delay(2000);

            IsBusy = false;
        }

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
                ((Command)RefreshDataCommand).ChangeCanExecute();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public class Grouping<K, T> : ObservableCollection<T>
        {
            public K Key { get; private set; }

            public Grouping(K key, IEnumerable<T> items)
            {
                Key = key;
                foreach (var item in items)
                    this.Items.Add(item);
            }
        }
    }
}