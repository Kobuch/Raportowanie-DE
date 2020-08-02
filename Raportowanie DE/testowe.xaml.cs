using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Raportowanie_DE.Strony
{
    /// <summary>
    /// Logika interakcji dla klasy testowe.xaml
    /// </summary>
    public partial class testowe : INotifyPropertyChanged
    {
            public event PropertyChangedEventHandler PropertyChanged;
        public string _searchTextText;

        public testowe()
        {
        
                ItemList = new ObservableCollection<string>();
                for (var i = 0; i < 100; i++)
                {
                    ItemList.Add($"Item {i}");
                }

                InitializeComponent();
            }

            private void Cb_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
            {
                cb.IsDropDownOpen = true;
            }

            public ObservableCollection<string> ItemList { get; set; }

            public string SearchTextText
            {
                get => _searchTextText;
                set
                {
                    if (_searchTextText == value) return;
                    _searchTextText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchTextText)));
                }
            }
        }

        
    


}
