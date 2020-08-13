using Raportowanie_DE.JPP_DEDataSetTableAdapters;
using Raportowanie_DE.Klasy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Excel = Microsoft.Office.Interop.Excel; 

namespace Raportowanie_DE.Okna
{
    /// <summary>
    /// Logika interakcji dla klasy Kontrola_pracownikow.xaml
    /// </summary>
    public partial class Kontrola_pracownikow : UserControl
    {
       

        private JPP_DEDataSet jPP_DEDataSet  = new JPP_DEDataSet();
        //private    View_zestaw_raporty_po_osobachTableAdapter view_Zestaw_Raporty_Po_OsobachTableAdapter = new View_zestaw_raporty_po_osobachTableAdapter();
        private View_zestaw_raporty_po_osobachTableAdapter view_Zestaw_Raporty_Po_OsobachTableAdapter = new View_zestaw_raporty_po_osobachTableAdapter();


        private LoginTableAdapter loginyTableAdapter = new LoginTableAdapter();
        private QueriesTableAdapter queriesTableAdapter = new QueriesTableAdapter();

        private string typ_pracownika = "pracownik";
        private string zespol = "DE11%";
        private Baza zestawienie = new Baza();
        private Pracownik pracownik1 = new Pracownik();
        private List<Pracownik> Pracowniks = new List<Pracownik>();

        private string imie1;
        private string nazwisko1;
        private string login1;
        private int nr_prac;
        private int week2;
        private int week1;
        private int week0;


        private bool _mail;

        private Excel.Application m_objExcel = null;
        private Excel.Workbooks m_objBooks = null;
        private Excel._Workbook m_objBook = null;
        private Excel.Sheets m_objSheets = null;
        private Excel._Worksheet m_objSheet = null;
        private Excel.Range m_objRange = null;
        private Excel.Font m_objFont = null;
        private Excel.QueryTables m_objQryTables = null;
        private Excel._QueryTable m_objQryTable = null;
        private object m_objOpt = System.Reflection.Missing.Value;
        private object m_strSampleFolder = "C:\\ExcelData\\";



        public Kontrola_pracownikow()
        {
            InitializeComponent();
            textBoxWeek.Text = zestawienie.WeekNumber.ToString();
          

        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
        wypelnij();
        }

        private void wypelnij()
        {

            loginyTableAdapter.FillBy_Typ_Stat_zespol (jPP_DEDataSet.Login, "Aktywny",zespol, "DE");
      

            DataRowCollection dataRows = jPP_DEDataSet.Login.Rows;
            zestawienie.pracownicy.Clear();
            Pracowniks.Clear();

            foreach (JPP_DEDataSet.LoginRow wiersz in dataRows)
            {

                // List<object> listawiersza = wiersz.ItemArray.ToList();
                imie1 = wiersz.Imie;
                nazwisko1 = wiersz.Nazwisko;
                login1 = wiersz.Login;
                nr_prac = wiersz.Nr_Pracownika;

                int? _week0 = queriesTableAdapter.SQLSumaGodz_Week_osoba(login1, zestawienie.WeekNumber);
                int? _week1 = queriesTableAdapter.SQLSumaGodz_Week_osoba(login1, zestawienie.WeekNumber - 1);
                int? _week2 = queriesTableAdapter.SQLSumaGodz_Week_osoba(login1, zestawienie.WeekNumber - 2);

                if (_week0 == null) week0 = 0; else week0 = (int)_week0;
                if (_week1 == null) week1 = 0; else week1 = (int)_week1;
                if (_week2 == null) week2 = 0; else week2 = (int)_week2;

                _mail = false;
                if (week0 < 40) _mail = true;


                zestawienie.pracownicy.Add(new Pracownik(imie1, nazwisko1, login1, nr_prac, week2, week1, week0, _mail));
                Pracowniks.Add(new Pracownik(imie1, nazwisko1, login1, nr_prac, week2, week1, week0, _mail));


            }


            Gweek0.Header = "week:" + zestawienie.WeekNumber.ToString();
            Gweek1.Header = "week:" + (zestawienie.WeekNumber - 1).ToString();
            Gweek2.Header = "week:" + (zestawienie.WeekNumber - 2).ToString();

            gridglowny.DataContext = Pracowniks;

            zestawieniedataGrid.Items.Refresh();

            // zestawieniedataGrid.ItemsSource = zestawienie.pracownicy;
            //  zestawieniedataGrid.ItemsSource = Pracowniks;




        }


        private void PrevWeek_Click(object sender, RoutedEventArgs e)
        {
            zestawienie.ZmianaWeek(-1);
            textBoxWeek.Text = zestawienie.WeekNumber.ToString();
            wypelnij();
        }

        private void nextWeek_Click(object sender, RoutedEventArgs e)
        {
            zestawienie.ZmianaWeek(1);
            textBoxWeek.Text = zestawienie.WeekNumber.ToString();
            wypelnij();
        }

        private void DoExcela_click(object sender, RoutedEventArgs e)
        {
            //Nagłówki
            List<string> naglowki = new List<string>();
            List<int> szerokosci = new List<int>();

            foreach (DataGridColumn kolumna in zestawieniedataGrid.Columns)
            {
                if (kolumna.Visibility == Visibility.Visible)
                {
                    naglowki.Add(kolumna.Header.ToString());
                    szerokosci.Add((int)kolumna.Width.Value / 5);
                }
            }


            //tresc
            List<string> wiersz1;
            Collection<List<string>> zestaw = new Collection<List<string>>();

            foreach (Pracownik Prac1 in zestawieniedataGrid.Items)
            {

                wiersz1 = new List<string>();
                wiersz1.Add(Prac1.imie.ToString());
                wiersz1.Add(Prac1.nazwisko.ToString());
                wiersz1.Add(Prac1.week2.ToString());
                wiersz1.Add(Prac1.week1.ToString());
                wiersz1.Add(Prac1.week0.ToString());
                zestaw.Add(wiersz1);

            }

            //Wywołanie działania klasy do obslugi excela

            ExcelAll excel1 = new ExcelAll();
            excel1.OpenNewExcel();
            excel1.PrzygotujNaglowki(naglowki, szerokosci);
            excel1.KopijDoExcela(zestaw);



        }

        private void button_Mail_Click(object sender, RoutedEventArgs e)
        {
            Poczta Email = new Poczta();

            List<string> Do = new List<string>();
            List<string> Cc = new List<string>();
            string tytul = "Przypomnienie o cotygodniowym raporcie za " + zestawienie.WeekNumber + " Tydzień";
            string tresc = "Poproszę o uzupełnienie brakujacych godzin. \n Ilosci zaraportowanych godzin:\n";
            foreach (Pracownik Prac1 in zestawieniedataGrid.Items)
            {
                if (Prac1.mail)
                {
                    Do.Add(Prac1.Login + "@jpp.pl");
                    tresc = tresc + Prac1.imie + " " + Prac1.nazwisko.Substring(0, 1) + ". :  " + Prac1.week0 + "\n";
                }
            }
            tresc = tresc + "\nPozdrawiam \n Kierownik";

            //// Email = new Poczta(Do, Cc, tytul, tresc);
            Email.PrzygotujWiadomosc(new Poczta(Do, Cc, tytul, tresc));






        }

        private void comboBoxAktyw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ComboBox obiekt1 = (sender as ComboBox);
            int wybor = obiekt1.SelectedIndex;

            if (wybor == 0) zespol = "DE111";
            if (wybor == 1) zespol = "DE112";
            if (wybor == 2) zespol = "DE11%";
            if (wybor == 3) zespol = "DE2%";
            if (wybor == 4) zespol = "DE10%";
            if (wybor == 5) zespol = "DE%";

            wypelnij(); 



        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            zestawieniedataGrid.ItemsSource = zestawienie.pracownicy;

            //view_Zestaw_Raporty_Po_OsobachTableAdapter.Fill(jPP_ArchitekciDataSet.View_zestaw_raporty_po_osobach);

            // view_zestaw_raporty_po_osobachDataGrid.ItemsSource = jPP_ArchitekciDataSet.View_zestaw_raporty_po_osobach;

        }

        private void butExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void zestawieniedataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //po osobach zestawienia

               DataGrid dataGrid = sender as DataGrid;

   
            Pracownik prac1 = dataGrid.CurrentCell.Item as Pracownik;

            przeglad_osoby przeglad_Osoby = new przeglad_osoby(prac1.Login);
            przeglad_Osoby.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            przeglad_Osoby.Show();


        }
    }


}

