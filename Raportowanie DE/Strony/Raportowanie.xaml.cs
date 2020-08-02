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
using Microsoft.Office.Interop.Outlook;
using Microsoft.Vbe.Interop;
using Raportowanie_DE.JPP_DEDataSetTableAdapters;
using Raportowanie_DE.Klasy;


namespace Raportowanie_DE.Strony
{
    /// <summary>
    /// Logika interakcji dla klasy Raportowanie.xaml
    /// </summary>
    /// 
 public partial class Raportowanie : UserControl
    {

        private JPP_DEDataSet jPP_DEDataSet = new JPP_DEDataSet();
        private JPP_DEDataSet jPP_DEDataSetJPP = new JPP_DEDataSet();

        private Zestawy_godzinTableAdapter zestawy_GodzinTableAdapter = new Zestawy_godzinTableAdapter();
        private QueriesTableAdapter queriesTableAdapter = new QueriesTableAdapter();
        private Lista_projektowTableAdapter lista_ProjektowTableAdapter = new Lista_projektowTableAdapter();
        private Lista_czynnosciTableAdapter lista_CzynnosciTableAdapter = new Lista_czynnosciTableAdapter();

    private System.Windows.Data.CollectionViewSource zestawy_godzinViewSource;
    private System.Windows.Data.CollectionViewSource zestawy_godzinViewSourceJPP;

        SolidColorBrush white1 = new SolidColorBrush(Colors.White);
        SolidColorBrush red1 = new SolidColorBrush(Colors.Red);

        public int Projekt_ID;
    public string nrwew;
    public string Projekt;
    public string Czynnosc;
    public bool dodaj;

    public string uprawnienie = "";
    public string osoba;

    public bool logowaniehaslem = false;
        Baza baza;



    #region START
   
        public Raportowanie(string osoba1)
        {

            //string login, string uprawnienie, bool logowZhaslem
            InitializeComponent();
        
            this.logowaniehaslem = true;
            this.osoba = osoba1;
            this.uprawnienie = "Admin";
            baza= new Baza();
                        
            
            textBoxWeek.Text = baza.WeekNumber.ToString();
            baza.ZmianaWeek(0);
            baza.login = osoba;
            labelOsoba.Content = osoba;
            calendar1.SelectedDates.AddRange(baza.dataweekStart, baza.dataweekEnd);

            lista_CzynnosciTableAdapter.FillBy_Rynek(jPP_DEDataSet.Lista_czynnosci, "JPP");


            zestawy_GodzinTableAdapter.FillBy_oso_week_rok(jPP_DEDataSet.Zestawy_godzin, baza.WeekNumber, baza.login, baza.RokNumber);
            zestawy_GodzinTableAdapter.FillByJPP(jPP_DEDataSetJPP.Zestawy_godzin, baza.WeekNumber, baza.login, baza.RokNumber);

            zestawy_godzinDataGrid.DataContext= jPP_DEDataSet.Zestawy_godzin;
           
           // zestawy_godzinDataGridJPP.ItemsSource = jPP_DEDataSetJPP.Zestawy_godzin;
            zestawy_godzinDataGrid.ItemsSource = jPP_DEDataSet.Zestawy_godzin;

            gridczynnosci.DataContext = jPP_DEDataSet.Lista_czynnosci;
           // odswierzaj();
            
        }

        #endregion


        #region Metody
        private void odswierzaj()
        //odświerza wszystkie wymagane kontrolki na ekranie(kalendarz, week, gridy, godziny)
        {
            textBoxWeek.Text = baza.WeekNumber.ToString();
            calendar1.SelectedDates.AddRange(baza.dataweekStart, baza.dataweekEnd);
            
            zestawy_GodzinTableAdapter.FillBy_oso_week_rok(jPP_DEDataSet.Zestawy_godzin,baza.WeekNumber, baza.login, baza.RokNumber);
            zestawy_GodzinTableAdapter.FillByJPP(jPP_DEDataSetJPP.Zestawy_godzin, baza.WeekNumber, baza.login, baza.RokNumber);

       
            zestawy_godzinDataGridJPP.ItemsSource = jPP_DEDataSetJPP.Zestawy_godzin;
            zestawy_godzinDataGrid.ItemsSource = jPP_DEDataSet.Zestawy_godzin;

            buttonZapisz.Visibility = Visibility.Hidden;
            butAnulujzmiany.Visibility = Visibility.Hidden;

            zliczajgodziny();
            labelRok.Content = baza.RokNumber;
        }


        private void zliczajgodziny()
        //Dokunuje zliczeń godzin w poszczególnych gridach i aktualizuje na ekranie kontrolki
        {
            List<object> rowlista;
            int sumagodzin = 0;
            int sumagodzin1 = 0;
            List<int> godziny = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (DataRowView row1 in zestawy_godzinDataGrid.Items)
            {
                Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow zestawrow = row1.Row as Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow;


                if (!zestawrow.IsPONNull()) { sumagodzin += zestawrow.PON; godziny[1] += zestawrow.PON; }
                if (!zestawrow.IsWTONull()) { sumagodzin += zestawrow.WTO; godziny[2] += zestawrow.WTO; }
                if (!zestawrow.IsSRONull()) { sumagodzin += zestawrow.SRO; godziny[3] += zestawrow.SRO; }
                if (!zestawrow.IsCZWNull()) { sumagodzin += zestawrow.CZW; godziny[4] += zestawrow.CZW; }
                if (!zestawrow.IsPIANull()) { sumagodzin += zestawrow.PIA; godziny[5] += zestawrow.PIA; }
                if (!zestawrow.IsSOBNull()) { sumagodzin += zestawrow.SOB; godziny[6] += zestawrow.SOB; }
                if (!zestawrow.IsNIEDNull()) { sumagodzin += zestawrow.NIED; godziny[7] += zestawrow.NIED; }

            }

            labelsumposr1.Content = sumagodzin;

            foreach (DataRowView row1 in zestawy_godzinDataGridJPP.Items)
            {
                Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow zestawrow = row1.Row as Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow;



                if (!zestawrow.IsPONNull()) sumagodzin1 += zestawrow.PON;
                if (!zestawrow.IsWTONull()) sumagodzin1 += zestawrow.WTO;
                if (!zestawrow.IsSRONull()) sumagodzin1 += zestawrow.SRO;
                if (!zestawrow.IsCZWNull()) sumagodzin1 += zestawrow.CZW;
                if (!zestawrow.IsPIANull()) sumagodzin1 += zestawrow.PIA;
            }

            labelsumposr2.Content = sumagodzin1;
            labelsumagodzin.Content = sumagodzin + sumagodzin1;

            if (sumagodzin + sumagodzin1 < 40) 
            { labelsumagodzin.Foreground = red1; }
            else
            { labelsumagodzin.Foreground = white1; }


                zestawy_godzinDataGrid.ColumnHeaderStyle = new Style();

            for (int k=1;k<=7;k++)
            {
                if (godziny[k]>=8)
                {
                    zestawy_godzinDataGrid.Columns[k+3].HeaderStyle = (Style)FindResource("myHeaderStylegreen");
                }
                else
                {
                    zestawy_godzinDataGrid.Columns[k + 3].HeaderStyle = (Style)FindResource("myHeaderStylewhite");
                }


            }
           // zestawy_godzinDataGrid.ColumnHeaderStyle = (Style)FindResource("myHeaderStyle");
           // zestawy_godzinDataGrid.Columns[2].HeaderStyle  = (Style)FindResource("myHeaderStylegreen");
           // zestawy_godzinDataGrid.Columns[3].HeaderStyle = (Style)FindResource("myHeaderStylewhite");

        }

        #endregion

        #region wywołania programu


        private void butWeekprev_Click(object sender, RoutedEventArgs e)
        {
            baza.ZmianaWeek(-1);
            odswierzaj();
        }

        private void butWeekNext_Click(object sender, RoutedEventArgs e)
        {
            baza.ZmianaWeek(1);
            odswierzaj();
        }

        private void calendar1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int tmp = baza.GetWeekNumber(calendar1.SelectedDate.Value);
            baza.WeekNumber = tmp;
            odswierzaj();
        }

        private void butusunProjekt_Click(object sender, RoutedEventArgs e)
        //usun wiersz z Grida
        {
            if ((zestawy_godzinDataGrid.Items.Count == 0) || (this.zestawy_godzinDataGrid.SelectedItem == null)) { return; }

                       
            
            DataRowView wiersz = (DataRowView)this.zestawy_godzinDataGrid.SelectedItem;
            Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow zestawrow = wiersz.Row as Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow;


            if ((baza.WeekNow - zestawrow.WEEK) > 4) { MessageBox.Show("Nie mozna usuwac pozycji starszych niż 4 tygodnie!"); return; }

            int godziny = 0;

            if (!zestawrow.IsPONNull())  godziny += zestawrow.PON;
            if (!zestawrow.IsWTONull()) godziny += zestawrow.WTO;
            if (!zestawrow.IsSRONull()) godziny += zestawrow.SRO;
            if (!zestawrow.IsCZWNull()) godziny += zestawrow.CZW;
            if (!zestawrow.IsPIANull()) godziny += zestawrow.PIA;
            if (!zestawrow.IsSOBNull()) godziny += zestawrow.SOB;
            if (!zestawrow.IsNIEDNull()) godziny += zestawrow.NIED;

            if (godziny > 0)
            {
                MessageBoxResult result1 = MessageBox.Show("Wiersz zawiera zarapotowane godziny.\n Proces nieodwracalny\n Czy napewno chcesz go usunać? ", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result1 == MessageBoxResult.No) { return; }
                if (result1 == MessageBoxResult.Yes) { usunwierszpoID(zestawrow.ID_Zestawy_godz); odswierzaj(); return; }
            }

            else
            {
                MessageBoxResult result1 = MessageBox.Show("Proces nieodwracalny\n Czy napewno chcesz usunać ten wiersz? ", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result1 == MessageBoxResult.No) { return; }
                if (result1 == MessageBoxResult.Yes) { usunwierszpoID(zestawrow.ID_Zestawy_godz); odswierzaj(); return; }
            }


           

        }

        private void usunwierszpoID(int ID)
        {
            var costam = queriesTableAdapter.DeleteSQL_ProjektzListyZadan_po_ID(ID);

        }

        private void zapisz()
        {
            if ((baza.WeekNow - baza.WeekNumber) > 4) { MessageBox.Show("Nie mozna dodawać pozycji dla raportów starszych niż 4 tygodnie!"); return; }

            zestawy_GodzinTableAdapter.Update(jPP_DEDataSet.Zestawy_godzin);
            zestawy_GodzinTableAdapter.Update(jPP_DEDataSetJPP.Zestawy_godzin);
            odswierzaj();
        }




    private void butDodajProjekt_Click(object sender, RoutedEventArgs e)
        {

          if ((baza.WeekNow - baza.WeekNumber) > 4) { MessageBox.Show("Nie mozna dodawać pozycji dla raportów starszych niż 4 tygodnie!"); return; }

            baza.dodaj_projektdolisty(osoba);

           odswierzaj();
        }








        private void butDodajJPP_Click(object sender, RoutedEventArgs e)
        {
            if ((baza.WeekNow - baza.WeekNumber) > 4) { MessageBox.Show("Nie mozna dodawać pozycji dla raportów starszych niż 4 tygodnie!"); return; }

            if (czynnoscComboBox.SelectedIndex == -1) return;
            
            
           


            //DataRowView aaaa = czynnoscComboBox.SelectedItem as DataRowView;
                       
            //JPP_DEDataSet.Lista_czynnosciRow lista1 = aaaa.Row as JPP_DEDataSet.Lista_czynnosciRow;

            //if (lista1.IsczynnoscNull()) return;

            DateTime? Data1 = null;

            string czynnosc1 = czynnoscComboBox.Text;
                
                
           //     lista1.czynnosc.ToString().TrimEnd();
                        
            if (this.queriesTableAdapter.SQL_Testzestaw_ma_projekt(1, czynnosc1, osoba, baza.RokNumber, baza.WeekNumber) != null)
            {
                MessageBox.Show("Ten projekt i zadanie sa juz na liście zadań. \n Wybierz inny.", "powielony", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            };


            try
            {
                zestawy_GodzinTableAdapter.Insert(1, "JPP", czynnosc1, "", osoba, Data1, baza.RokNumber, baza.WeekNumber, null, null, null, null, null, null, null, false, true, "JPP0001", "");
                
            }
            catch (System.Exception)
            {
                MessageBox.Show("problem z dodaniem projektu do listy raportowej, skontaktuj sie z administatorem");
            }


            odswierzaj();


        }

        private void buttonZapisz_Click(object sender, RoutedEventArgs e)
        {
            if ((baza.WeekNow - baza.WeekNumber) > 4) { MessageBox.Show("Nie mozna dodawać pozycji dla raportów starszych niż 4 tygodnie!"); return; }

            zapisz();
        }

        private void zestawy_godzinDataGridJPP_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            buttonZapisz.Visibility = Visibility.Visible;
            butAnulujzmiany.Visibility = Visibility.Visible;
        }

        private void butAnulujzmiany_Click(object sender, RoutedEventArgs e)
        {
            odswierzaj();
        }

        private void zestawy_godzinDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)

        {
                             
            buttonZapisz.Visibility = Visibility.Visible;
            butAnulujzmiany.Visibility = Visibility.Visible;
        }

        private void butusunczynnoscJPP_Click(object sender, RoutedEventArgs e)
        {
                     
            
            if ((zestawy_godzinDataGridJPP.Items.Count == 0) || (this.zestawy_godzinDataGridJPP.SelectedItem == null)) { return; }

         
            DataRowView wiersz = (DataRowView)this.zestawy_godzinDataGridJPP.SelectedItem;
            Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow zestawrow = wiersz.Row as Raportowanie_DE.JPP_DEDataSet.Zestawy_godzinRow;


            if ((baza.WeekNow - zestawrow.WEEK) > 4) { MessageBox.Show("Nie mozna usuwac pozycji starszych niż 4 tygodnie!"); return; }

            int godziny = 0;

            if (!zestawrow.IsPONNull()) godziny += zestawrow.PON;
            if (!zestawrow.IsWTONull()) godziny += zestawrow.WTO;
            if (!zestawrow.IsSRONull()) godziny += zestawrow.SRO;
            if (!zestawrow.IsCZWNull()) godziny += zestawrow.CZW;
            if (!zestawrow.IsPIANull()) godziny += zestawrow.PIA;
            

            if (godziny > 0)
            {
                MessageBoxResult result1 = MessageBox.Show("Wiersz zawiera zarapotowane godziny.\n Proces nieodwracalny\n Czy napewno chcesz go usunać? ", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result1 == MessageBoxResult.No) { return; }
                if (result1 == MessageBoxResult.Yes) { usunwierszpoID(zestawrow.ID_Zestawy_godz); odswierzaj(); return; }
            }

            else
            {
                MessageBoxResult result1 = MessageBox.Show("Proces nieodwracalny\n Czy napewno chcesz usunać ten wiersz? ", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result1 == MessageBoxResult.No) { return; }
                if (result1 == MessageBoxResult.Yes) { usunwierszpoID(zestawrow.ID_Zestawy_godz); odswierzaj(); return; }
            }


        }

        private void butZPoprzedniego_Click(object sender, RoutedEventArgs e)
        //kopiuje liste projektów z poprzdniego tygodnia do bierzącego. Bez projektów JPP
        {
            if (zestawy_godzinDataGridJPP.Items.Count > 0)
            { MessageBox.Show("Opcja niedostepna jeżeli  w bieżącym tygodniu jest już jakaś pozycja na liście.", "Powielanie pozycji", MessageBoxButton.OK, MessageBoxImage.Warning); return; };

            queriesTableAdapter.SQL_Skopiowanie_projektow_last_week(baza.RokNumber.ToString(), baza.WeekNumber.ToString(), baza.login, baza.RokNumber, baza.WeekNumber - 1);
            odswierzaj();
        }

        private void butDodajJPPL4Urlop_Click(object sender, RoutedEventArgs e)
        //dodaje na szybko pozycje L4 i urlop
        {
            DateTime? Data1 = null;

            try
            {
                if (this.queriesTableAdapter.SQL_Testzestaw_ma_projekt(1, "L4", osoba, baza.RokNumber, baza.WeekNumber) == null)
                {
                    zestawy_GodzinTableAdapter.Insert(1, "JPP", "L4", "", osoba, Data1, baza.RokNumber, baza.WeekNumber, null, null, null, null, null, null, null, false, true, "JPP0001", "");


           
                }
                if (this.queriesTableAdapter.SQL_Testzestaw_ma_projekt(1, "Urlop Wypocz", osoba, baza.RokNumber, baza.WeekNumber) == null)
                {
                    zestawy_GodzinTableAdapter.Insert(1, "JPP", "Urlop Wypocz", "", osoba, Data1, baza.RokNumber, baza.WeekNumber, null, null, null, null, null, null, null, false, true, "JPP0001", "");

                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("problem z dodaniem projektu do listy raportowej, skontaktuj sie z administatorem");
            }

            odswierzaj();

        }




        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            odswierzaj();
            // Nie ładuj danych w czasie projektowania.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Tu załaduj swoje dane i przypisz wynik do CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

      
    }
}
