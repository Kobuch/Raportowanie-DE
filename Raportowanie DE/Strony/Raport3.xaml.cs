using Microsoft.Office.Interop.Outlook;
using Raportowanie_DE.Klasy;
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
    /// Logika interakcji dla klasy Raport3.xaml
    /// </summary>
    public partial class Raport3 : UserControl
    {

        private JPP_DEDataSet jPP_DEDataSet = new JPP_DEDataSet();
        private JPP_DEDataSetTableAdapters.Zestawy_godzinTableAdapter zestawy_GodzinTableAdapter = new JPP_DEDataSetTableAdapters.Zestawy_godzinTableAdapter();
        private JPP_DEDataSetTableAdapters.LoginTableAdapter loginTableAdapter = new JPP_DEDataSetTableAdapters.LoginTableAdapter();
        private List<string> listaosob = new List<string>();
        private List<JPP_DEDataSet.Zestawy_godzinRow> lista_zestawygodzRow = new List<JPP_DEDataSet.Zestawy_godzinRow>();
        private List<JPP_DEDataSet.Zestawy_godzinRow> lista_zestawygodzRowview = new List<JPP_DEDataSet.Zestawy_godzinRow>();
        private ExcelAll excelAll = new ExcelAll();
        

        public Raport3()
        {
            InitializeComponent();
            string zespol = "DE11%";
            loginTableAdapter.FillBy_Typ_Stat_zespol(jPP_DEDataSet.Login, "Aktywny", zespol, "DE");
            loginComboBox.ItemsSource = jPP_DEDataSet.Login;

        }

        private void loginComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ComboBox box = sender as ComboBox;

             if (box.SelectedIndex <0 ) return;

            DataRowView dataRow = box.SelectedItem as DataRowView;   
    
            JPP_DEDataSet.LoginRow row = dataRow.Row as JPP_DEDataSet.LoginRow;
           

            przygotuj_zestaw(row.Login);

            view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid.DataContext = lista_zestawygodzRowview;

        }

        private void przygotuj_zestaw(string login1)
        {
           
                lista_zestawygodzRow.Clear();
                zestawy_GodzinTableAdapter.FillBy_login(jPP_DEDataSet.Zestawy_godzin, login1);
                //wypełnij liste 
                foreach (JPP_DEDataSet.Zestawy_godzinRow row in jPP_DEDataSet.Zestawy_godzin)
                {
                    lista_zestawygodzRow.Add(row);  
                }
                //wykonaj obliczenia na lioscie i usun powielające
                oblicz(lista_zestawygodzRow);

                         
        }

        private void uruchom()
        {
         


        }



        private void oblicz(List<JPP_DEDataSet.Zestawy_godzinRow> rows)
        {
            JPP_DEDataSet.Zestawy_godzinRow row;
            JPP_DEDataSet.Zestawy_godzinRow row1;

            int weeknim= rows.Min( wier => wier.WEEK);
            List<string> listanumerow = new List<string>();
            int sumagodzin = 0;

            for (int i=0 ; i < rows.Count-1 ; i++)
            {
                row = rows[i];
                string nr_proj = row.Numer;
                string czynnosc = row.Czynnosc;
                string klient = row.Klient;
                string operat = row.Rezerwa2;
                //sumagodzin godzin    
                    for (int k=i+1; k<rows.Count;k++)
                        {
                            if ((rows[k].Numer == row.Numer) && (rows[k].Czynnosc==row.Czynnosc) && (rows[k].Klient==row.Klient) && (rows[k].Rezerwa2==row.Rezerwa2))
                    {
                        //sumuj godziny
                        row1 = rows[k];
                        int suma1 = obliczsumeweek(row);
                        int suma2 = obliczsumeweek(row1);

                        if (suma1 >= suma2)
                        {
                            rows.Remove(row1);
                            k = k - 1;
                        }
                        else
                        {
                            rows.Remove(row);
                            i = i - 1;
                        }

                    }

            }
                    
            }
            lista_zestawygodzRowview = rows;
            view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid.DataContext = null;
            view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid.DataContext = lista_zestawygodzRowview;
            

        }
        private int obliczsumeweek(JPP_DEDataSet.Zestawy_godzinRow row)
        {
            int pomocnicza = 0;
            if (!row.IsPONNull()) pomocnicza += row.PON;
            if (!row.IsWTONull()) pomocnicza += row.WTO;
            if (!row.IsSRONull()) pomocnicza += row.SRO;
            if (!row.IsCZWNull()) pomocnicza += row.CZW;
            if (!row.IsPIANull()) pomocnicza += row.PIA;
            if (!row.IsSOBNull()) pomocnicza += row.SOB;
            if (!row.IsNIEDNull()) pomocnicza += row.NIED;
                        return pomocnicza;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Nie ładuj danych w czasie projektowania.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Tu załaduj swoje dane i przypisz wynik do CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
            // Nie ładuj danych w czasie projektowania.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Tu załaduj swoje dane i przypisz wynik do CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void butprzygraport_Click(object sender, RoutedEventArgs e)
        {
            uruchom();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void do_excel_Click(object sender, RoutedEventArgs e)
        {


            DataGrid aaaa = view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid as DataGrid;

            Clipboard.Clear();
            DataObject data = new DataObject();
            data.SetData(DataFormats.Text, "My Stuff with headers");
            Clipboard.SetDataObject(data);

        
        }

        private void view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            //Clipboard.Clear();
            //DataObject data = new DataObject();
            //data.SetData(DataFormats.Text, "My Stuff with headers");
            //Clipboard.SetDataObject(data);
        }

       
    }
}
