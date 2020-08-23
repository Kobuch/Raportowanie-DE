using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Reflection;
using System.Collections.ObjectModel;
using Raportowanie_DE.JPP_DEDataSetTableAdapters;
using System.Data;
using System.Windows.Controls;

namespace Raportowanie_DE.Klasy
{

    class Baza
    {



        public int WeekNumber = 20;
        public int WeekNow = 20;
        public int RokNumber = DateTime.Now.Year;
        public string login = "";

        public DateTime dataweekStart = DateTime.Now;
        public DateTime dataweekEnd = DateTime.Now;

        public Pracownik pracownik = new Pracownik();
        public List<Pracownik> pracownicy = new List<Pracownik>();
        public List<Poczta> Maile = new List<Poczta>();


       
        public Baza()
        {
            WeekNumber = GetWeekNumber(DateTime.Now);
            WeekNow = GetWeekNumber(DateTime.Now);


        }

        public void addpracownikdolisty(Pracownik prac1)
        {
            pracownicy.Add(prac1);
        }



        public void ZmianaWeek(int zmiana)
        {
            //ustawia datę pierwszą jak jest arg1=true, aktualizuje pole z nr week i kalendarz

            WeekNumber = WeekNumber + zmiana;

            if (WeekNumber > 53) { WeekNumber = 1; RokNumber += 1; }
            if (WeekNumber < 1) { WeekNumber = 53; RokNumber -= 1; }

            dataweekStart = GetDayfromweek(RokNumber, WeekNumber, 1).AddDays(1);
            dataweekEnd = GetDayfromweek(RokNumber, WeekNumber, 1).AddDays(7);
        }
        public int GetWeekNumber(DateTime dzien)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dzien, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        public DateTime GetDayfromweek(int Year1, int week1, int dayOfWeek)
        {
            DateTime jan1 = new DateTime(Year1, 1, 1);
            int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = week1;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstMonday.AddDays(weekNum * 7 + dayOfWeek - 2);
            return result;
        }

        public void dodaj_projektdolisty(string osoba1)
        {
            Okna.Przypisanie_projektu Projekt_Do_Listy = new Okna.Przypisanie_projektu();


            Projekt_Do_Listy.osoba = osoba1;
            Projekt_Do_Listy.week = WeekNumber;
            Projekt_Do_Listy.rok = RokNumber;

            Projekt_Do_Listy.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Projekt_Do_Listy.ShowDialog();
        }
        

    

    }
    class Pracownik
    {

        public string imie { get; set; }
        public string nazwisko { get; set; }
        public int wynik { get; set; }
        public string Login { get; set; }
        public int nr_pracownika { get; set; }

        public int week0
        { get; set; }

        public int week1 { get; set; }

        public int week2 { get; set; }
        public string Typ_prcownika { get; set; }

        public bool mail { get; set; }

        public Pracownik()
        { }

        public Pracownik(string imie1, string nazwisko1, string _login, int _nr_prac, int _week2, int _week1, int _week0, bool _mail)
        {
            this.imie = imie1;
            this.nazwisko = nazwisko1;
            this.Login = _login;
            this.nr_pracownika = _nr_prac;
            this.week2 = _week2;
            this.week1 = _week1;
            this.week0 = _week0;
            this.mail = _mail;

        }

    }

    class ExcelAll
    {


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


        public ExcelAll()
        { }

        public void OpenNewExcel()
        {
            m_objExcel = new Excel.Application();
            m_objExcel.Visible = true;
            m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
            m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));
            m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
            m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));

        }
        public void PrzygotujNaglowki(List<string> naglowki, List<int> szerokosci)
        {
         

            m_objSheet.Rows[1].Font.Bold = true;
            for (int i = 0; i < szerokosci.Count; i++)
            {
                m_objSheet.Columns[i + 1].ColumnWidth = szerokosci[i];
            }
            for (int i = 0; i < naglowki.Count; i++)
            {
                m_objSheet.Cells[1, i + 1] = naglowki[i];
            }




        }
        public void KopijDoExcela(Collection<List<string>> zestaw)
        {
            int k = 2;
            int j = 1;
            foreach (List<string> wiersz1 in zestaw)
            {
                j = 1;
                foreach (string wyraz in wiersz1)
                {

                    m_objSheet.Cells[k, j] = wyraz;
                    j++;
                }
                k++;

            }


        }

        public String STR_do_listy(object wyrazenie)
        {
            if ((wyrazenie == null) || (wyrazenie == DBNull.Value))
            {
                return "";

            }

            string slowo = wyrazenie.ToString();
            return slowo;
        }

        public void K_O_W_R_P_CDataTabletonewexcel(JPP_DEDataSet.View_Zest2_sumagodzin_K_O_W_R_P_CDataTable table)
        {
            OpenNewExcel();

               for (int i = 1; i < table.Columns.Count + 1; i++)
                    {
                      m_objSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                      
            }

                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        for (int k = 0; k < table.Columns.Count; k++)
                        {
                    m_objSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                        }
                    }

        }

        public void K_O_W_RCDataTabletonewexcel(JPP_DEDataSet.View_Zest1_sumagodzin_K_O_W_RDataTable table)
        {
            OpenNewExcel();

            for (int i = 1; i < table.Columns.Count + 1; i++)
            {
                m_objSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;

            }

            for (int j = 0; j < table.Rows.Count; j++)
            {
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    m_objSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                }
            }

        }


       public void view_Zest3_Sumagodzin_Osoba_ProjTableAdaptertonewexcel(JPP_DEDataSet.View_Zest3_sumagodzin_Osoba_projDataTable table)
        {
            OpenNewExcel();

            for (int i = 1; i < table.Columns.Count + 1; i++)
            {
                m_objSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;

            }

            for (int j = 0; j < table.Rows.Count; j++)
            {
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    m_objSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                }
            }

        }


        public void tabelatoexcel (DataTable table)
        {
            OpenNewExcel();

            for (int i = 1; i < table.Columns.Count + 1; i++)
            {
                m_objSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;

            }

            for (int j = 0; j < table.Rows.Count; j++)
            {
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    m_objSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                }
            }

        }



    }







    class Poczta
    {

        public string DO;
        public string CC;
        public string Tytul;
        public string Tresc;
        List<string> DO_lista = new List<string>();

        public Poczta()
        { }
        public Poczta(string _DO, string _CC, string _Tytul, string _Tresc)
        {
            this.DO = _DO;
            this.CC = _CC;
            this.Tresc = _Tresc;
            this.Tytul = _Tytul;
        }

        public Poczta(List<string> _DO_list, List<string> _CC_list, string _Tytul, string _Tresc)
        {
            this.DO = "";
            foreach (string adres in _DO_list)
            {
                this.DO = this.DO + "; " + adres;
            }

            this.CC = "";
            foreach (string adres in _CC_list)
            {
                this.CC = this.CC + "; " + adres;
            }
            this.Tresc = _Tresc;
            this.Tytul = _Tytul;
        }

        public void PrzygotujWiadomosc(Poczta _Email)
        {
            try
            {
                Outlook.Application eMail = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)eMail.CreateItem(Outlook.OlItemType.olMailItem);

                mailItem.To = _Email.DO;
                mailItem.CC = _Email.CC;
                mailItem.Subject = _Email.Tytul;

                mailItem.Body = _Email.Tresc;

                mailItem.Display(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }


        }
        public void PrzygotujWiadomosc()
        {

            try
            {

                Outlook.Application oApp = new Outlook.Application();

                // Get the MAPI namespace.
                Outlook.NameSpace oNS = oApp.GetNamespace("mapi");

                // Log on by using the default profile or existing session (no dialog box).
                oNS.Logon(Missing.Value, Missing.Value, false, true);

                // Alternate logon method that uses a specific profile name.
                // TODO: If you use this logon method, specify the correct profile name
                // and comment the previous Logon line.
                //oNS.Logon("profilename",Missing.Value,false,true);

                //Get the Inbox folder.
                Outlook.MAPIFolder oInbox = oNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);

                //Get the Items collection in the Inbox folder.
                Outlook.Items oItems = oInbox.Items;

                // Get the first message.
                // Because the Items folder may contain different item types,
                // use explicit typecasting with the assignment.
                Outlook.MailItem oMsg = (Outlook.MailItem)oItems.GetFirst();

                //Output some common properties.
                Console.WriteLine(oMsg.Subject);
                Console.WriteLine(oMsg.SenderName);
                Console.WriteLine(oMsg.ReceivedTime);
                Console.WriteLine(oMsg.Body);

                //Check for attachments.
                int AttachCnt = oMsg.Attachments.Count;
                Console.WriteLine("Attachments: " + AttachCnt.ToString());

                //TO DO: If you use the Microsoft Outlook 10.0 Object Library, uncomment the following lines.
                /*if (AttachCnt > 0) 
                {
                for (int i = 1; i <= AttachCnt; i++) 
                 Console.WriteLine(i.ToString() + "-" + oMsg.Attachments.Item(i).DisplayName);
                }*/

                //TO DO: If you use the Microsoft Outlook 11.0 Object Library, uncomment the following lines.
                /*if (AttachCnt > 0) 
                {
                for (int i = 1; i <= AttachCnt; i++) 
                 Console.WriteLine(i.ToString() + "-" + oMsg.Attachments[i].DisplayName);
                }*/


                //Display the message.
                oMsg.Display(true);  //modal

                //Log off.
                oNS.Logoff();

                //Explicitly release objects.
                oMsg = null;
                oItems = null;
                oInbox = null;
                oNS = null;
                oApp = null;
            }

            //Error handler.
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught: ", e);
            }








            //try
            //{
            //    Outlook.Application eMail = new Outlook.Application();
            //    Outlook.MailItem mailItem = (Outlook.MailItem)eMail.CreateItem(Outlook.OlItemType.olMailItem);

            //    mailItem.To = DO;
            //    mailItem.CC = CC;
            //    mailItem.Subject = Tytul;
            //    mailItem.Body = Tresc;


            //    mailItem.Display(true);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}


        }
    }

    class Logowanieclass
    {

        private string Username = "";
        private string Password = "";
        private string aktywnosc = "";
        private bool mozna_logowac = false;
        private bool _zalogowanyhaslem = false;


        public string Login { get { return Username; } set { Username = value; } }
        public string Haslo { set { Password = value; } }
        public string Uprawnienie { get { return aktywnosc; } }
        public bool Zalogowany { get { return mozna_logowac; } }
        public bool ZalogowanyHaslem { get { return _zalogowanyhaslem; } }

        private QueriesTableAdapter queriesTableAdapter = new QueriesTableAdapter();
        private LoginTableAdapter loginyTableAdapter = new LoginTableAdapter();
        private JPP_DEDataSet  jPP_DEDataSet = new JPP_DEDataSet();

        public Logowanieclass()
        {

        }

        public bool ZgodnoscLogin()
        {
            if (queriesTableAdapter.SQL_Login(Login) != null)
            {
                string _warunek = queriesTableAdapter.SQL_Login(Login).ToString();
                if (_warunek == "Aktywny")
                {
                    loginyTableAdapter.FillBy_bylogin(jPP_DEDataSet.Login, Login);
                    JPP_DEDataSet.LoginRow wiersz2 = jPP_DEDataSet.Login.First();
                    aktywnosc = wiersz2.Aktywnosc.ToString();
                    if (wiersz2.IsAktywnoscNull()) aktywnosc = ""; else aktywnosc = wiersz2.Aktywnosc.ToString();

                    return true;
                }
                else
                {
                    MessageBox.Show("Pracownik nieaktywny, skontaktuj sie z administratorem", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
            return false;
        }

        public int ZgodnoscLogPass()
        //sprawdzenie czy jest zgodny login i hasło w bazie View_loginy
        {

            //czy zgodny login
            if (queriesTableAdapter.SQL_Login(Login) != null)
            {
                string _warunek = queriesTableAdapter.SQL_Login(Login).ToString();
                if (_warunek == "Aktywny")
                {
                    if (queriesTableAdapter.SQL_Password_verify(Login, Password) != null)
                    {
                        string _warunek2 = queriesTableAdapter.SQL_Password_verify(Login, Password).ToString();

                        //aktywny i zgodne hasło, jest OK
                        aktywnosc = _warunek2.ToString();
                        _zalogowanyhaslem = true;
                        return 1;
                        //close
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {

                    return 3;
                }

            }
            else
            {
                return 4;
            }
        }






    }



}
