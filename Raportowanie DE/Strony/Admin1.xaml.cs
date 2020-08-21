using Raportowanie_DE.JPP_DEDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Raportowanie_DE.Strony
{
    /// <summary>
    /// Logika interakcji dla klasy Admin1.xaml
    /// </summary>
    public partial class Admin1 : UserControl
    {


        JPP_DEDataSet jPP_DEDataSet = new JPP_DEDataSet();
        Zestawy_godzinTableAdapter zestawy_GodzinTableAdapter = new Zestawy_godzinTableAdapter();
        public Admin1()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            zestawy_GodzinTableAdapter.Fill(jPP_DEDataSet.Zestawy_godzin);
            zestawy_godzinDataGrid.ItemsSource = jPP_DEDataSet.Zestawy_godzin;

            // Nie ładuj danych w czasie projektowania.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Tu załaduj swoje dane i przypisz wynik do CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void butedutuj_click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (sender as Button).DataContext as DataRowView;
            JPP_DEDataSet.Zestawy_godzinRow wiersz = dataRowView.Row as JPP_DEDataSet.Zestawy_godzinRow;

            zestawy_GodzinTableAdapter.FillBy_ID(jPP_DEDataSet.Zestawy_godzin, wiersz.ID_Zestawy_godz);
            zestawy_godzinDataGrid.ItemsSource = jPP_DEDataSet.Zestawy_godzin;


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (zestawy_godzinDataGrid.Items.Count==1)
            {
                zestawy_GodzinTableAdapter.Update(jPP_DEDataSet.Zestawy_godzin);

                zestawy_GodzinTableAdapter.Fill(jPP_DEDataSet.Zestawy_godzin);
            }
        }
    }
}
