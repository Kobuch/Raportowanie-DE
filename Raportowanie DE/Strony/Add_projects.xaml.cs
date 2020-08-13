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
using Raportowanie_DE.JPP_DEDataSetTableAdapters;


namespace Raportowanie_DE.Strony
{
    /// <summary>
    /// Logika interakcji dla klasy Add_projects.xaml
    /// </summary>
    public partial class Add_projects : UserControl
    {

        Raportowanie_DE.JPP_DEDataSet jPP_DEDataSet = new JPP_DEDataSet();
        Lista_projektowTableAdapter lista_ProjektowTableAdapter = new Lista_projektowTableAdapter();
        QueriesTableAdapter queriesTableAdapter = new QueriesTableAdapter();

        slownik_AktywnyTableAdapter slownik_AktywnyTableAdapter = new slownik_AktywnyTableAdapter();
        slownik_statusTableAdapter slownik_StatusTableAdapter = new slownik_statusTableAdapter();
        Slownik_klienciTableAdapter slownik_KlienciTableAdapter = new Slownik_klienciTableAdapter();
        Slownik_operatorTableAdapter slownik_OperatorTableAdapter = new Slownik_operatorTableAdapter();

        private List<Wierszprojektu> lista1 = new List<Wierszprojektu>();

        public Add_projects()
        {
            InitializeComponent();
            lista1.Clear();

        }

        private void Zmieniaj()
            //zamienia bloktextowy na wartosci w gridview
        {
                       
            {
                lista1.Clear();
                

                string copiedContent = textBoxDane.Text;
                
                foreach (string row in copiedContent.Split('\n'))
                {
                    Wierszprojektu wierszgrid = new Wierszprojektu();
                       if (!string.IsNullOrEmpty(row))
                    {
                        int i = 0;
                        foreach (string cell in row.Split(' '))
                        {
                            if (i == 0) wierszgrid.numer = cell.TrimEnd();
                            if (i == 1) wierszgrid.Klient = cell.TrimEnd();
                            if (i == 2) wierszgrid.Operator= cell.TrimEnd();
                            i++;
                        }
                        wierszgrid.powielony = false;
                        wierszgrid.niewgrano = false;
                        lista1.Add(wierszgrid);
                      
                    }
                }
                Gridglowny.DataContext = lista1;
                lista_projektowDataGrid.Items.Refresh();
                labelIlerekordow.Content = lista_projektowDataGrid.Items.Count.ToString() + ": rekordow";


            }
        }



        private void ZapiszZmianydobazy()
            //przenosi warosci z grid do bazy

        {
            
            foreach (Wierszprojektu wierszprojektu in lista_projektowDataGrid.Items  )
            {
                //sprawdz czy już jest w bazie taki projekt

                if (this.queriesTableAdapter.SQL_CzyjestProjektNaLiscie_numer(wierszprojektu.numer) == null)
                {
                    try
                    {
                        lista_ProjektowTableAdapter.Insert(wierszprojektu.numer, wierszprojektu.numer, "", wierszprojektu.Klient, wierszprojektu.Operator, "", "", "Aktywny", false);
                        wierszprojektu.niewgrano = false;
                        wierszprojektu.powielony = false;
                    }
                    catch (Exception)
                    {
                        wierszprojektu.niewgrano = true;
                      
                    }
                }
                else
                {
                    wierszprojektu.powielony = true;
                }

            }
            //usuwanie przeniesioonych do bazy
            
            

            for (int k= lista_projektowDataGrid.Items.Count-1; k>=0 ; k--)
            {
                Wierszprojektu wierszprojektu = lista_projektowDataGrid.Items[k] as Wierszprojektu;
                if ((!wierszprojektu.powielony)&& (!wierszprojektu.niewgrano))  lista1.Remove(wierszprojektu);
            }

            lista_projektowDataGrid.Items.Refresh();
          
            labelIlerekordow.Content = lista_projektowDataGrid.Items.Count.ToString() + ": rekordow";
            MessageBox.Show("wykonano");

        }


        private void butzmiana_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxDane.Text.Length > 1)
            {
                lista_projektowDataGrid.CanUserAddRows = false;
                Zmieniaj();
                
                //textBoxDane.Text = "";

            }
        }
        private void butZapiszdobazy_Click(object sender, RoutedEventArgs e)
        {
            if (lista_projektowDataGrid.Items.Count > 0) ZapiszZmianydobazy();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = textBoxDane;

            int lineNumber = 2;


            txtBox.SelectionStart = txtBox.GetCharacterIndexFromLineIndex(lineNumber - 1);
            txtBox.SelectionLength = txtBox.GetLineLength(lineNumber - 1);
            
           // txtBox.CaretIndex = txtBox.SelectionStart;
           // txtBox.ScrollToLine(lineNumber - 1);
           // txtBox.Focus();



        }

        private void dodajwiersz_click(object sender, RoutedEventArgs e)
        {

            lista_projektowDataGrid.CanUserAddRows = false;

            //  lista1.Add(new Wierszprojektu());
            Wierszprojektu wierszprojektu = new Wierszprojektu();
            lista1.Clear();
            lista1.Add(wierszprojektu);
            Gridglowny.DataContext = lista1;
            lista_projektowDataGrid.Items.Refresh();
        

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            slownik_KlienciTableAdapter.Fill(jPP_DEDataSet.Slownik_klienci);
            klientComboBox.ItemsSource = jPP_DEDataSet.Slownik_klienci;
            slownik_OperatorTableAdapter.Fill(jPP_DEDataSet.Slownik_operator);
            operatorComboBox.ItemsSource = jPP_DEDataSet.Slownik_operator;


        }

        private void buttondodajpojedynczy_Click(object sender, RoutedEventArgs e)
        {


           // lista1.Clear();

                Wierszprojektu wierszgrid = new Wierszprojektu();
                if ( textboxnumer.Text.Length>1)
                {
                wierszgrid.numer = textboxnumer.Text;
                wierszgrid.Klient = klientComboBox.Text;
                wierszgrid.Operator = operatorComboBox.Text;
                lista1.Add(wierszgrid);       
                }

            if (lista1.Count != 0)
            {
                Gridglowny.DataContext = lista1;

                lista_projektowDataGrid.Items.Refresh();
                lista_projektowDataGrid.CanUserAddRows = false;
                labelIlerekordow.Content = lista_projektowDataGrid.Items.Count.ToString() + ": rekordow";
                
            }


        }
    }

    public class  Wierszprojektu
        {

            public string numer { get; set; }
            public string Klient { get; set; }
            public string Operator { get; set; }
            public bool niewgrano { get; set; }
            public bool powielony { get; set; }



        public Wierszprojektu()

            {
                numer = "";
                Klient = "";
                Operator = "";
                niewgrano = false;
                powielony = false;
            }

        }

   
}
