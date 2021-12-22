using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace Pixel
{
    /// <summary>
    /// Логика взаимодействия для New_Client.xaml
    /// </summary>
    public partial class New_Client : Window
    {
        public New_Client()
        {
            InitializeComponent();
            //for (int i = 0; i < 5; i++)
            //{
            //    ComboBox_CountVizit.Items.Add(i+1);
            //}
            //ComboBox_CountVizit.Items.Add("5+");
           
          
        }
        public static Dictionary<string, int> ClientsSerch = new Dictionary<string, int>();
        public void Button_Click()
        {
            //доб  в бузу клиентов,сорт

          




        }
        public static bool IsAllDigits(string s)
        {
            int t = 0;
            foreach (char c in s)
            {
                if (!Char.IsDigit(c))    return false;
                if (t ==0 && c=='7')     return false;
                else t++;
            }
            return true;
        }
        public void AddCleintToFile()
        {
            var xml = new XmlSerializer(typeof(List<Guest>));
            xml.Serialize(new StreamWriter("CientBaza.xml"),General.guests);
           
        }
        private void ButtonAddNewClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Guest guest = new Guest();
                if (  TextBox_number.Text == "")
                {
                    MessageBox.Show("Анкета должна содержать как минимум телефон");return;
                }
                if (TextBox_NameCliient.Text != "") guest.Name = TextBox_NameCliient.Text.ToUpper();
                DateTime dt;
                string myDate = TexBox_lastVizit.Text;
                bool success = DateTime.TryParse(myDate, out dt);
                if (success) guest.LastVisit = myDate;
                else
                {
                    if (myDate != "") { MessageBox.Show("Введите корректно дату"); return; }                   
                }

                if ( IsAllDigits(TextBox_number.Text)) guest.Phone = TextBox_number.Text;
                else { MessageBox.Show("Телефон введен некорректно"); return; }

                if (CountVizitBox.Text!="")guest.VisitCount = int.Parse(CountVizitBox.Text);
                guest.Comment = TextBoxComm.Text;            
                if (!guest.SerchPhone(guest, General.guests)) { MessageBox.Show("Анкета с  номером " + guest.Phone + " уже есть в базе"); return; }
                else { General.guests.Add(guest); AddCleintToFile(); }
            }
            catch 
            {
                MessageBox.Show("Данные введены некорректно"); return;
            }
            this.Close();

        }
    }
}
