using Raportowanie_DE.Klasy;
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

namespace Raportowanie_DE
{
    /// <summary>
    /// Logika interakcji dla klasy Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window

        //ekran logowania wynikiem jego jest wywołanie dalszego okna z parametrami ( Uprawnienia, osoba, zalogowany z hasłem)

    {

        #region Zmienne
        Logowanieclass logowanie = new Logowanieclass();
        private bool wyborlogowaniazhaslem = false; 
        #endregion

        #region START

        public Logowanie()
        {
            InitializeComponent();

            labelOsoba.Content = Environment.UserName;
            //ustawia login na podstawie domeny

            labelPas_Copy.Visibility = Visibility.Hidden;
            labellog_Copy.Visibility = Visibility.Hidden;
            textBoxLogin.Visibility = Visibility.Hidden;
            textBoxPasword.Visibility = Visibility.Hidden;
            butLogowDomena_Copy.Visibility = Visibility.Hidden;
            labelpassw.Visibility = Visibility.Hidden;
            labellogin.Visibility = Visibility.Hidden;
        }

        #endregion

        #region wywołania
        private void butLogowDomena_Click(object sender, RoutedEventArgs e)


        {
            logowanie.Login = labelOsoba.Content.ToString();

            if (logowanie.ZgodnoscLogin())
            {

                //mozna upriścic jeżeli otwierany jest ten sam program. ale tak nie będzie w przyszlosci.

                if ((logowanie.Uprawnienie == "KierArch") || (logowanie.Uprawnienie == "Koordynator") || (logowanie.Uprawnienie == "Admin"))
                {
                    MainWindow mainWindow = new MainWindow(logowanie.Uprawnienie, logowanie.Login, false);
                                  //( logowanie.Uprawnienie,logowanie.Login, logowanie.ZalogowanyHaslem);
                    mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(logowanie.Uprawnienie, logowanie.Login, false);
      
                    mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    mainWindow.Show();
                    this.Close();

                }
               // else MessageBox.Show("Brak uprawnień do zalogowania. skontaktuj sie z Administratorem");
            }
            else MessageBox.Show("Nie jesteś zalogowany do domeny JPP, zaloguj sie za pomoca opcji Logowanie przy użyciu hasła");
        }

        private void LogowanieZHaslem(object sender, MouseButtonEventArgs e)
        {
            if (wyborlogowaniazhaslem)
            {
                labelPas_Copy.Visibility = Visibility.Hidden;
                labellog_Copy.Visibility = Visibility.Hidden;
                textBoxLogin.Visibility = Visibility.Hidden;
                textBoxPasword.Visibility = Visibility.Hidden;
                butLogowDomena_Copy.Visibility = Visibility.Hidden;
                butLogowDomena.Visibility = Visibility.Visible;
                label2.Visibility = Visibility.Visible;
                labelOsoba.Visibility = Visibility.Visible;

                
                wyborlogowaniazhaslem = false;
            }
            else
            {
                labelPas_Copy.Visibility = Visibility.Visible;
                labellog_Copy.Visibility = Visibility.Visible;
                textBoxLogin.Visibility = Visibility.Visible;
                textBoxPasword.Visibility = Visibility.Visible;
                butLogowDomena_Copy.Visibility = Visibility.Visible;
                butLogowDomena.Visibility = Visibility.Hidden;
                label2.Visibility = Visibility.Hidden;
                labelOsoba.Visibility = Visibility.Hidden;

                textBoxLogin.Text = labelOsoba.Content.ToString();

                wyborlogowaniazhaslem = true;
            }
           
        }

        private void butLogowDomena_Copy_Click(object sender, RoutedEventArgs e)
        {
            //logowanie z hasłem
            labelpassw.Visibility = Visibility.Hidden;
            labellogin.Visibility = Visibility.Hidden;

            logowanie.Login = this.textBoxLogin.Text;
            logowanie.Haslo = this.textBoxPasword.Password;
            int wynik = logowanie.ZgodnoscLogPass();

            if (wynik == 2) { labelpassw.Visibility = Visibility.Visible; return; }
            if (wynik == 4) { labellogin.Visibility = Visibility.Visible; return; }
            if (wynik == 3) { MessageBox.Show("Pracownik nieaktywny, skontaktuj sie z administratorem", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            if (wynik == 1)
            {

                if ((logowanie.Uprawnienie == "kierownik") || (logowanie.Uprawnienie == "koordynator") || (logowanie.Uprawnienie == "admin"))
                {
                    MainWindow mainWindow = new MainWindow(logowanie.Uprawnienie, logowanie.Login,true);
                   // mainWindow.Osoba = logowanie.Login;
                    mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    mainWindow.Show();
                    
                }

                else
                {

                    MainWindow mainWindow = new MainWindow(logowanie.Uprawnienie, logowanie.Login, true);
                  //  mainWindow.Osoba = logowanie.Login;
                    mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    mainWindow.Show();

                }

                this.Close();
            }

        }
        #endregion



    }
}
