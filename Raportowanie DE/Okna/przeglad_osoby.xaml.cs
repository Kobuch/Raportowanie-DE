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
using System.Windows.Shapes;
using Raportowanie_DE.JPP_DEDataSetTableAdapters;
using Raportowanie_DE.Klasy;

namespace Raportowanie_DE.Okna
{
    /// <summary>
    /// Logika interakcji dla klasy przeglad_osoby.xaml
    /// </summary>
    public partial class przeglad_osoby : Window
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

        public przeglad_osoby(string osoba1)
        {
            InitializeComponent();
            this.logowaniehaslem = true;
            this.osoba = osoba1;
            this.uprawnienie = "Admin";
            baza = new Baza();


            textBoxWeek.Text = baza.WeekNumber.ToString();
            baza.ZmianaWeek(0);
            baza.login = osoba;
            labelOsoba.Content = osoba;
            calendar1.SelectedDates.AddRange(baza.dataweekStart, baza.dataweekEnd);

            lista_CzynnosciTableAdapter.FillBy_Rynek(jPP_DEDataSet.Lista_czynnosci, "JPP");


            zestawy_GodzinTableAdapter.FillBy_oso_week_rok(jPP_DEDataSet.Zestawy_godzin, baza.WeekNumber, baza.login, baza.RokNumber);
            zestawy_GodzinTableAdapter.FillByJPP(jPP_DEDataSetJPP.Zestawy_godzin, baza.WeekNumber, baza.login, baza.RokNumber);

            zestawy_godzinDataGrid.DataContext = jPP_DEDataSet.Zestawy_godzin;

            // zestawy_godzinDataGridJPP.ItemsSource = jPP_DEDataSetJPP.Zestawy_godzin;
            zestawy_godzinDataGrid.ItemsSource = jPP_DEDataSet.Zestawy_godzin;

            gridczynnosci.DataContext = jPP_DEDataSet.Lista_czynnosci;
            // odswierzaj();
        }

        #region Metody
        private void odswierzaj()
        //odświerza wszystkie wymagane kontrolki na ekranie(kalendarz, week, gridy, godziny)
        {
            textBoxWeek.Text = baza.WeekNumber.ToString();
            calendar1.DisplayDate = baza.dataweekStart;
            calendar1.SelectedDates.AddRange(baza.dataweekStart, baza.dataweekEnd);


            zestawy_GodzinTableAdapter.FillBy_oso_week_rok(jPP_DEDataSet.Zestawy_godzin, baza.WeekNumber, baza.login, baza.RokNumber);
            zestawy_GodzinTableAdapter.FillByJPP(jPP_DEDataSetJPP.Zestawy_godzin, baza.WeekNumber, baza.login, baza.RokNumber);


            zestawy_godzinDataGridJPP.ItemsSource = jPP_DEDataSetJPP.Zestawy_godzin;
            zestawy_godzinDataGrid.ItemsSource = jPP_DEDataSet.Zestawy_godzin;



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

            for (int k = 1; k <= 7; k++)
            {
                if (godziny[k] >= 8)
                {
                    zestawy_godzinDataGrid.Columns[k + 3].HeaderStyle = (Style)FindResource("myHeaderStylegreen");
                }
                else
                {
                    zestawy_godzinDataGrid.Columns[k + 3].HeaderStyle = (Style)FindResource("myHeaderStylewhite");
                }


            }


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




        private void buttonZapisz_Click(object sender, RoutedEventArgs e)
        {
            // if ((baza.WeekNow - baza.WeekNumber) > 4) { MessageBox.Show("Nie mozna dodawać pozycji dla raportów starszych niż 4 tygodnie!"); return; }

            zapisz();
        }

      

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            odswierzaj();
         
        }



    }
}
