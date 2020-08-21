using Raportowanie_DE.Klasy;
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
    /// Logika interakcji dla klasy Raport1.xaml
    /// </summary>
    public partial class Raport2 : UserControl
    {
        JPP_DEDataSet jPP_DEDataSet = new JPP_DEDataSet();
        JPP_DEDataSetTableAdapters.View_Zest3_sumagodzin_Osoba_projTableAdapter view_Zest3_Sumagodzin_Osoba_ProjTableAdapter = new JPP_DEDataSetTableAdapters.View_Zest3_sumagodzin_Osoba_projTableAdapter();      
 

        Baza raportstart = new Baza();
        Baza raportkoniec = new Baza();
        ExcelAll excelAll = new ExcelAll();


        public Raport2()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           // view_Zest1_Sumagodzin_K_O_W_RTableAdapter.Fill(jPP_DEDataSet.View_Zest1_sumagodzin_K_O_W_R);
           // view_Zest1_Sumagodzin_K_O_W_RTableAdapter.FillBy_weekstart_i_koniec(jPP_DEDataSet.View_Zest1_sumagodzin_K_O_W_R,2020, 31, 31);
                      
            DateTime minimum = new DateTime(2020, 7, 27);
            calendarstart.DisplayDateStart = minimum;
            calendarkoniec.DisplayDateStart = minimum;
            odswierzaj();
        }

        private void odswierzaj()
        {
            textBoxWeekstart.Text = raportstart.WeekNumber.ToString();
            raportstart.ZmianaWeek(0);
            calendarstart.DisplayDate = raportstart.dataweekStart;
            calendarstart.SelectedDates.AddRange(raportstart.dataweekStart, raportstart.dataweekEnd);
            labelRokstart.Content = raportstart.RokNumber;

            textBoxWeekkoniec.Text = raportkoniec.WeekNumber.ToString();
            raportkoniec.ZmianaWeek(0);
            calendarkoniec.DisplayDate = raportkoniec.dataweekStart;
            calendarkoniec.SelectedDates.AddRange(raportkoniec.dataweekStart, raportkoniec.dataweekEnd);
            labelRokkoniec.Content = raportkoniec.RokNumber;

        }

       
       

        private void butWeekprev_Click(object sender, RoutedEventArgs e)
        {
            if ((raportstart.RokNumber <= 2020) && (raportstart.WeekNumber <= 31)) return;
            raportstart.ZmianaWeek(-1);
            odswierzaj();
        }

        private void butWeekNext_Click(object sender, RoutedEventArgs e)
        {
            if (raportstart.RokNumber > 2020) return;
            raportstart.ZmianaWeek(1);
            odswierzaj();
        }

        private void calendarstart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int tmp = raportstart.GetWeekNumber(calendarstart.SelectedDate.Value);
            raportstart.WeekNumber = tmp;
            raportstart.RokNumber = calendarstart.SelectedDate.Value.Year;

            odswierzaj();
        }

        private void butWeekNextkoniec_Click(object sender, RoutedEventArgs e)
        {
            if (raportkoniec.RokNumber > 2020) return;
            raportkoniec.ZmianaWeek(1);
            odswierzaj();
        }

        private void butWeekprevkoniec_Click(object sender, RoutedEventArgs e)
        {
            if ((raportkoniec.RokNumber <= 2020) && (raportkoniec.WeekNumber <= 31)) return;
            raportkoniec.ZmianaWeek(-1);
            odswierzaj();
        }

        private void calendarkoniec_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int tmp = raportkoniec.GetWeekNumber(calendarkoniec.SelectedDate.Value);
            raportkoniec.WeekNumber = tmp;
            raportkoniec.RokNumber = calendarkoniec.SelectedDate.Value.Year;


            odswierzaj();
        }

        private void przygoruj_raport_Click(object sender, RoutedEventArgs e)
        {
            if (raportstart.RokNumber> raportkoniec.RokNumber) { MessageBox.Show("Data startu mniejsza od daty końca, popraw daty"); return; }
            if (raportstart.WeekNumber > raportkoniec.WeekNumber) { MessageBox.Show("Data startu mniejsza od daty końca, popraw daty"); return; }

             view_Zest3_Sumagodzin_Osoba_ProjTableAdapter.FillBy_rokweek_start_end(jPP_DEDataSet.View_Zest3_sumagodzin_Osoba_proj, raportstart.RokNumber*100+raportstart.WeekNumber, raportstart.RokNumber * 100+raportkoniec.WeekNumber);

            view_Zest3_sumagodzin_Osoba_projDataGrid.DataContext = jPP_DEDataSet.View_Zest3_sumagodzin_Osoba_proj;
         
        }

        private void doexcela_Click(object sender, RoutedEventArgs e)
        {

                        excelAll.view_Zest3_Sumagodzin_Osoba_ProjTableAdaptertonewexcel(jPP_DEDataSet.View_Zest3_sumagodzin_Osoba_proj);
    
        }
    }

 


}
