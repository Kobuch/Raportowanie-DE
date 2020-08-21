using Raportowanie_DE.Okna;
using Raportowanie_DE.Strony;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Raportowanie_DE
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public string Osoba { get; set; }
        private string osoba1 = "";




        Raportowanie raportowanie;

        Add_projects add_Projects = new Add_projects();
       Kontrola_pracownikow kontrola_Pracownikow = new Kontrola_pracownikow();
        slowniki slowniki = new slowniki();
        Zestawienie1 zestawienie1 = new Zestawienie1();
        Raport1 raportgodzin1 = new Raport1();
        Raport2 raportbyosoby = new Raport2();
        Admin1 raportadmin1 = new Admin1();


        private string uprawnienie;


        public MainWindow(string Uprawnienie, string Osoba1, bool logzhaslem)
        {

            InitializeComponent();
           
            this.osoba1 = Osoba1;
            raportowanie = new Raportowanie(osoba1);
            ContentControl_JPP.Content = raportowanie;
            this.uprawnienie = Uprawnienie;
            

       

        }
        private void MainWindow_loaded(object sender, RoutedEventArgs e)
        {
            if(uprawnienie== "koordynator")
            {
                button2.IsEnabled = true;
                button2.Visibility = Visibility.Visible;
                button3.IsEnabled = true;
                button3.Visibility = Visibility.Visible;
                button4.IsEnabled = true;
                button4.Visibility = Visibility.Visible;
                button5.IsEnabled = true;
                button5.Visibility = Visibility.Visible;
                button6.IsEnabled = true;
                button6.Visibility = Visibility.Visible;
            }

            if ((uprawnienie == "kierownik") || (uprawnienie == "admin"))
            {
                button2.IsEnabled = true;
                button2.Visibility = Visibility.Visible;
                button3.IsEnabled = true;
                button3.Visibility = Visibility.Visible;
                button4.IsEnabled = true;
                button4.Visibility = Visibility.Visible;
                button5.IsEnabled = true;
                button5.Visibility = Visibility.Visible;
                button6.IsEnabled = true;
                button6.Visibility = Visibility.Visible;
                button7.IsEnabled = true;
                button7.Visibility = Visibility.Visible;
                buttonAdmin1.IsEnabled = true;
                buttonAdmin1.Visibility = Visibility.Visible;
            }

        }

        private void Polecenie1_click(object sender, RoutedEventArgs e)
        {
            ContentControl_JPP.Content = raportowanie;
            labelglowny.Content = "Raportowanie";
        }

        private void Polecenie2_click(object sender, RoutedEventArgs e)
        {
            ContentControl_JPP.Content = add_Projects;
            labelglowny.Content = "Dodawanie projektów";
        }

        private void Polecenie3_Click(object sender, RoutedEventArgs e)
        {
            
            ContentControl_JPP.Content = kontrola_Pracownikow; ;
            labelglowny.Content = "Kontrola raportowania godzin";


        }

        private void Polecenie4_Click(object sender, RoutedEventArgs e)
        {

            ContentControl_JPP.Content = slowniki;
            labelglowny.Content = "Słowniki";


        }
        private void Exit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Polecenie5_Click(object sender, RoutedEventArgs e)
        {
            ContentControl_JPP.Content = zestawienie1;
            labelglowny.Content = "Raport 1. Zestawienie godzin";
        }

        private void Polecenie6_Click(object sender, RoutedEventArgs e)
        {
            ContentControl_JPP.Content = raportgodzin1;
            labelglowny.Content = "Raport 2. Zestawienie godzin";
        }

        private void Polecenie7_Click(object sender, RoutedEventArgs e)
        {
            ContentControl_JPP.Content = raportbyosoby;
            labelglowny.Content = "Raport 3. Zestawienie godzin na osoby";

        }

        private void PolecenieAdmin1_Click(object sender, RoutedEventArgs e)
        {
            ContentControl_JPP.Content = raportadmin1;
            labelglowny.Content = "Raport Admin1. Edycja rekordów";
        }
    }
}
