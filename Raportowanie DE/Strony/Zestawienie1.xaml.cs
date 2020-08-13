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
    /// Logika interakcji dla klasy Zestawienie1.xaml
    /// </summary>
    public partial class Zestawienie1 : UserControl
    {

        private JPP_DEDataSet jPP_DEDataSet = new JPP_DEDataSet();
        private View_zestawienie_na_czynnosciTableAdapter view_Zestawienie_Na_CzynnosciTableAdapter = new View_zestawienie_na_czynnosciTableAdapter();
        private View_Zestawienie_na_projektyTableAdapter view_Zestawienie_Na_ProjektyTableAdapter = new View_Zestawienie_na_projektyTableAdapter();

        private Lista_projektowTableAdapter lista_ProjektowTableAdapter = new Lista_projektowTableAdapter();
        private string nr_wew = "";


        public Zestawienie1()
        {
            InitializeComponent();

            view_Zestawienie_Na_ProjektyTableAdapter.Fill(jPP_DEDataSet.View_Zestawienie_na_projekty);
            view_Zestawienie_Na_CzynnosciTableAdapter.Fill(jPP_DEDataSet.View_zestawienie_na_czynnosci);
            lista_ProjektowTableAdapter.Fill(jPP_DEDataSet.Lista_projektow);

            gridGlobal.DataContext = jPP_DEDataSet.View_zestawienie_na_czynnosci;
            grid2.DataContext = jPP_DEDataSet.Lista_projektow;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

    
        }

        private void numer_wewComboBox_selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox przycisk = sender as ComboBox;

            if ((przycisk.SelectedIndex != -1) && (przycisk.SelectedIndex!=0))
            {
                DataRowView aaaa = przycisk.SelectedValue as DataRowView;
                JPP_DEDataSet.Lista_projektowRow xxx = aaaa.Row as JPP_DEDataSet.Lista_projektowRow;

                nr_wew = xxx.numer.ToString();

                view_Zestawienie_Na_CzynnosciTableAdapter.FillBy_by_nr_wew(jPP_DEDataSet.View_zestawienie_na_czynnosci, nr_wew);
                view_Zestawienie_Na_ProjektyTableAdapter.FillBy_by_nr_wew(jPP_DEDataSet.View_Zestawienie_na_projekty, nr_wew);
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox wybor = sender as CheckBox;
            if (wybor.IsChecked == true)
            {
                gridGlobal.DataContext = jPP_DEDataSet.View_zestawienie_na_czynnosci;

            }
            else
            {
                gridGlobal.DataContext = jPP_DEDataSet.View_Zestawienie_na_projekty;
            }
        }
    }
}
