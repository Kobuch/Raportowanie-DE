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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Raportowanie_DE.Strony
{
    /// <summary>
    /// Logika interakcji dla klasy slowniki.xaml
    /// </summary>
    public partial class slowniki : UserControl
    {


        JPP_DEDataSet jPP_DEDataSet;
        slownik_AktywnyTableAdapter slownik_AktywnyTableAdapter = new slownik_AktywnyTableAdapter();
        slownik_statusTableAdapter slownik_StatusTableAdapter = new slownik_statusTableAdapter();
        Slownik_klienciTableAdapter slownik_KlienciTableAdapter = new Slownik_klienciTableAdapter();
        Slownik_operatorTableAdapter slownik_OperatorTableAdapter = new Slownik_operatorTableAdapter();


        public slowniki()
        {
            InitializeComponent();
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           

            jPP_DEDataSet = ((Raportowanie_DE.JPP_DEDataSet)(this.FindResource("jPP_DEDataSet")));

            odswierzaj();



        }

        private void odswierzaj()
        {
            slownik_AktywnyTableAdapter.Fill(jPP_DEDataSet.slownik_Aktywny);
            slownik_StatusTableAdapter.Fill(jPP_DEDataSet.slownik_status);
            slownik_KlienciTableAdapter.Fill(jPP_DEDataSet.Slownik_klienci);
            slownik_OperatorTableAdapter.Fill(jPP_DEDataSet.Slownik_operator);

        }

        private void buttonklient_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxKlient.Text.Length>1)
            {
                slownik_KlienciTableAdapter.Insert(textBoxKlient.Text);
                slownik_KlienciTableAdapter.Fill(jPP_DEDataSet.Slownik_klienci);
                textBoxKlient.Text = "";
            }
        }

        private void buttonOperator_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxoperator.Text.Length>1)
            {
                slownik_OperatorTableAdapter.Insert(textBoxoperator.Text);
                slownik_OperatorTableAdapter.Fill(jPP_DEDataSet.Slownik_operator);
                textBoxoperator.Text = "";
            }


        }

        private void buttonStatus_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxStatus.Text.Length>1)
            {
                slownik_StatusTableAdapter.Insert(textBoxStatus.Text);
                slownik_StatusTableAdapter.Fill(jPP_DEDataSet.slownik_status);
                textBoxStatus.Text = "";
            }
        }

        private void buttonAktywny_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxAktywny.Text.Length>1)
            {
                slownik_AktywnyTableAdapter.Insert(textBoxAktywny.Text);
                slownik_AktywnyTableAdapter.Fill(jPP_DEDataSet.slownik_Aktywny);
                textBoxAktywny.Text = "";
            }


        }
    }
}
