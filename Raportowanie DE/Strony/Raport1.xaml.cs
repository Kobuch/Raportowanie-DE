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
    /// Logika interakcji dla klasy Raport1.xaml
    /// </summary>
    public partial class Raport1 : UserControl
    {
        JPP_DEDataSet jPP_DEDataSet = new JPP_DEDataSet();
        JPP_DEDataSetTableAdapters.View_Zest1_sumagodzin_K_O_W_RTableAdapter view_Zest1_Sumagodzin_K_O_W_RTableAdapter = new JPP_DEDataSetTableAdapters.View_Zest1_sumagodzin_K_O_W_RTableAdapter();
        JPP_DEDataSetTableAdapters.View_Zest2_sumagodzin_K_O_W_R_P_CTableAdapter view_Zest2_Sumagodzin_K_O_W_R_P_CTableAdapter = new JPP_DEDataSetTableAdapters.View_Zest2_sumagodzin_K_O_W_R_P_CTableAdapter();
        public Raport1()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            view_Zest1_Sumagodzin_K_O_W_RTableAdapter.Fill(jPP_DEDataSet.View_Zest1_sumagodzin_K_O_W_R);
            //view_Zest2_Sumagodzin_K_O_W_R_P_CTableAdapter.Fill(jPP_DEDataSet.View_Zest2_sumagodzin_K_O_W_R_P_C);

           // view_Zest1_Sumagodzin_K_O_W_RTableAdapter.FillBy_weekstart_i_koniec(jPP_DEDataSet.View_Zest1_sumagodzin_K_O_W_R,2020, 31, 31);
            view_Zest2_Sumagodzin_K_O_W_R_P_CTableAdapter.FillBy_rok_weekstart_end(jPP_DEDataSet.View_Zest2_sumagodzin_K_O_W_R_P_C, 2020, 30, 33);

            view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid.DataContext = jPP_DEDataSet.View_Zest2_sumagodzin_K_O_W_R_P_C;

            // Nie ładuj danych w czasie projektowania.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Tu załaduj swoje dane i przypisz wynik do CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox wybor = sender as CheckBox;
            if (wybor.IsChecked == true)
            {
                view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid.DataContext = jPP_DEDataSet.View_Zest2_sumagodzin_K_O_W_R_P_C;

            }
            else
            {
                view_Zest2_sumagodzin_K_O_W_R_P_CDataGrid.DataContext = jPP_DEDataSet.View_Zest1_sumagodzin_K_O_W_R;
            }
        }
    }
}
