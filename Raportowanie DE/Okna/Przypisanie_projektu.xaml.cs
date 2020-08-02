using Raportowanie_DE.JPP_DEDataSetTableAdapters;
using System;
using System.Collections.Generic;
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

namespace Raportowanie_DE.Okna
{
    /// <summary>
    /// Logika interakcji dla klasy Przypisanie_projektu.xaml
    /// </summary>
    public partial class Przypisanie_projektu : Window
    {

        private QueriesTableAdapter queriesTableAdapter = new QueriesTableAdapter();
        private Zestawy_godzinTableAdapter zestawy_GodzinTableAdapter = new Zestawy_godzinTableAdapter();
        private JPP_DEDataSet jPP_DEDataSet;
        private Lista_projektowTableAdapter jPP_DEDataSetLista_projektowTableAdapter;

        public string osoba;
        public int week;
        public int rok;



        public Przypisanie_projektu()
        {
            InitializeComponent();
            bord1.Visibility = Visibility.Hidden;
            bord2.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            jPP_DEDataSet = ((Raportowanie_DE.JPP_DEDataSet)(this.FindResource("jPP_DEDataSet")));
            // Załaduj dane do tabeli Lista_projektow. Możesz modyfikować ten kod w razie potrzeby.
            jPP_DEDataSetLista_projektowTableAdapter = new Raportowanie_DE.JPP_DEDataSetTableAdapters.Lista_projektowTableAdapter();
            //    jPP_DEDataSetLista_projektowTableAdapter.Fill(jPP_DEDataSet.Lista_projektow);
            jPP_DEDataSetLista_projektowTableAdapter.FillBy_Aktywne(jPP_DEDataSet.Lista_projektow, "Aktywny");

          


          //  System.Windows.Data.CollectionViewSource lista_projektowViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lista_projektowViewSource")));
            //lista_projektowViewSource.View.MoveCurrentToFirst();
            // Załaduj dane do tabeli Lista_czynnosci. Możesz modyfikować ten kod w razie potrzeby.
            Raportowanie_DE.JPP_DEDataSetTableAdapters.Lista_czynnosciTableAdapter jPP_DEDataSetLista_czynnosciTableAdapter = new Raportowanie_DE.JPP_DEDataSetTableAdapters.Lista_czynnosciTableAdapter();
            //jPP_DEDataSetLista_czynnosciTableAdapter.Fill(jPP_DEDataSet.Lista_czynnosci);
            jPP_DEDataSetLista_czynnosciTableAdapter.FillBy_Rynek(jPP_DEDataSet.Lista_czynnosci,"DE");


            System.Windows.Data.CollectionViewSource lista_czynnosciViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lista_czynnosciViewSource")));
           // lista_czynnosciViewSource.View.MoveCurrentToFirst();
        }

        private void button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            

             if ((numerTextBox.Text.TrimEnd() == "")||(czynnoscTextBox.Text.TrimEnd() == "")) 
            {
                bord1.Visibility = Visibility.Visible;
                bord2.Visibility = Visibility.Visible;

            MessageBox.Show("Musisz wybrac Projekt i Czynność", "uwaga", MessageBoxButton.OK, MessageBoxImage.Warning); 
            
            bord1.Visibility = Visibility.Hidden;
            bord2.Visibility = Visibility.Hidden;
            return;   
            }

            int Projekt_ID = Int32.Parse(iD_lista_projTextBox.Text.TrimEnd());
            string nrwew = numerTextBox.Text.TrimEnd();
            string operator1 = operatorTextBox.Text.TrimEnd();
            string klient1 = klientTextBox.Text.TrimEnd();    
         
            string Czynnosc = czynnoscTextBox.Text.TrimEnd();
            DateTime? Data1 = null;

          



            if (this.queriesTableAdapter.SQL_Testzestaw_ma_projekt(Projekt_ID, Czynnosc, osoba, rok, week) != null)
            {
                MessageBox.Show("Ten projekt i zadanie sa juz na liście zadań. \n Wybierz inny.", "powielony", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            };


            try
            {
                zestawy_GodzinTableAdapter.Insert(Projekt_ID, nrwew, Czynnosc, klient1, osoba, Data1, rok, week, null, null, null, null, null, null, null, false, true, nrwew, operator1);
                   this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("problem z dodaniem projektu do listy raportowej, skontaktuj sie z administatorem");
            }
        }

        private void button_anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void butt2_click(object sender, RoutedEventArgs e)
        {

        }

        private void butAddNewProj_Click(object sender, RoutedEventArgs e)
        {

        }

        private void burAddDalej_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonszukaj_Click(object sender, RoutedEventArgs e)
        {
            //to do szukaj na liscie
            textBoxszukaj.Text = "";
            jPP_DEDataSetLista_projektowTableAdapter.FillBy_Aktywne(jPP_DEDataSet.Lista_projektow, "Aktywny");

        }

        

        private void textBoxszukaj_KeyUp(object sender, KeyEventArgs e)
        {

            TextBox textBox = sender as TextBox;

            if (textBox.Text.Length > 0)
            {

                try
                {
                    string szukana = textBox.Text ;
                    jPP_DEDataSetLista_projektowTableAdapter.FillBy_by_numer_aktywny(jPP_DEDataSet.Lista_projektow, textBoxszukaj.Text, "Aktywny");

                }
                catch (Exception)
                {

                    throw;
                }

            }

        }
    }
}
