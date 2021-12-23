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
    /// Логика взаимодействия для General.xaml
    /// </summary>
    public partial class General : Window
    {
        public static List<Bron> result = new List<Bron>();
        DateTime? DateFilter1, DateFilter2 = null;
        public static List<Guest> guests = new List<Guest>();
        List<Tovar> tovars = new List<Tovar>();
       
        internal static Google.Apis.Auth.OAuth2.UserCredential credential;
        internal static Google.Apis.Sheets.v4.SheetsService service;

    

        public static List<Prodaza> prodazZal = new List<Prodaza>();
        public static Otchet otchet = new Otchet();


        
        public General(string s)
        {
            InitializeComponent();
            //кнопки
            Grid_Add_Product.Visibility = Visibility.Hidden;

            ComboBoxProducs.Items.Add("пиво 100");
            NameAdmin.Content = s;
            var now = DateTime.Now;
            var xml = new XmlSerializer(typeof(List<Guest>));
            guests = (List<Guest>)xml.Deserialize(new StreamReader("CientBaza.xml"));
    

            var nowplus6 = now.AddDays(6);
            Button_DateFrom.Content = string.Format("{0} - {1}", now.ToShortDateString(), nowplus6.ToShortDateString());
            credential = GoogleAPI.GetUserCredentials();
            service = GoogleAPI.GetService(credential);
            result.AddRange(GoogleAPI.GetDataGoogle(service, now, nowplus6));
            Data_Grid_Baza_Clientov.ItemsSource = guests;
            DataGrid_prodaza.ItemsSource = otchet.OtchetPoProdazamProductov;
            grid1.ItemsSource = result;
            DataGrid_otchet.ItemsSource = otchet.AllProdaz;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Prodaza.IsEnabled = true;
            Baza.IsEnabled = true;
            Statistic.IsEnabled = true;
            Otchet.IsEnabled = true;

            Grid_Statistic.Visibility = Visibility.Hidden;
            Grid_Add_Product.Visibility = Visibility.Hidden;
            Grid_Baza.Visibility = Visibility.Hidden;
            Grid_Calend.Visibility = Visibility.Visible;
            Otchets.Visibility = Visibility.Hidden;
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

     
        private void Button_Click_Prodazha(object sender, RoutedEventArgs e)
        {
            Grid_Calend.Visibility = Visibility.Hidden;
            Grid_Baza.Visibility = Visibility.Hidden;
            Grid_Statistic.Visibility = Visibility.Hidden;
            Otchets.Visibility = Visibility.Hidden;
            Grid_Add_Product.Visibility = Visibility.Visible;

            Baza.IsEnabled = true;
            Statistic.IsEnabled = true;
            Otchet.IsEnabled = true;
            Button_Prodaza.IsEnabled = false;
            //откр новый грид с анкетой продаж



        }
        private void Statistic_Click(object sender, RoutedEventArgs e)
        {
            Grid_Calend.Visibility = Visibility.Hidden;
        }

        private void B_new_b_Click(object sender, RoutedEventArgs e)
        {
            //открывается единое окно для методов "добавить" и "изменить", но во 2 случае поля предзаполнены
        }

        private void B_chan_Click(object sender, RoutedEventArgs e)
        {
            //открывается единое окно для методов "добавить" и "изменить", но во 2 случае поля предзаполнены
            //после изменения обновление данных везде: в гриде и в гугл!!!
        }

        private void Button_DateFrom_Click(object sender, RoutedEventArgs e)
        {
            Calendar.Visibility = Visibility.Visible;
        }


        private void B_new_b_Click_1(object sender, RoutedEventArgs e)
        {
            var b = grid1.SelectedItem as Bron;
            if (b is null)
            {
                MessageBox.Show("poka");
                return;
            }
            new New_bron(b).ShowDialog();
            grid1.Items.Refresh();
        }


        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxSposobOpl.SelectedItem == null) { MessageBox.Show("Некорректно сделан выбор"); return; }
                string[] s = ComboBoxProducs.SelectedItem.ToString().Split(' ');
                Tovar t = new Tovar(s[0].ToString(), int.Parse(s[1]));
                var dateTime = DateTime.Now;

                if (ComboBoxSposobOpl.Text.ToString() != "Смешанная оплата")
                {
                    var d = ComboBoxSposobOpl.Text.ToString();
                    otchet.OtchetPoProdazamProductov.Add(new Prodaza(t, ComboBoxSposobOpl.Text.ToString(), dateTime.ToString()));
                    if (otchet.SposobOpl.ContainsKey("evo") && d == "Эвотор") otchet.SposobOpl["evo"] += t.Cost;
                    if (otchet.SposobOpl.ContainsKey("nal") && d == "Наличные") otchet.SposobOpl["nal"] += t.Cost;
                    if (otchet.SposobOpl.ContainsKey("per") && d == "Перевод") otchet.SposobOpl["per"] += t.Cost;



                }
                else
                {
                    string sm_opl = "смешанная оплата ";
                    if (TextBox_SmOpl_evo.Text != "0")
                    {
                        sm_opl += TextBox_SmOpl_evo.Text + " эвотор ";
                        if (otchet.SposobOpl.ContainsKey("evo")) otchet.SposobOpl["evo"] += int.Parse(TextBox_SmOpl_evo.Text);
                    }
                    if (TextBox_SmOPl_Nal.Text != "0")
                    {
                        sm_opl += TextBox_SmOPl_Nal.Text + " наличными ";
                        if (otchet.SposobOpl.ContainsKey("nal")) otchet.SposobOpl["nal"] += int.Parse(TextBox_SmOPl_Nal.Text);
                    }
                    if (TextBox_SmOpl_per.Text != "0") sm_opl += TextBox_SmOpl_per.Text + " переводом";
                    {
                        sm_opl += " =" + s[1];
                        if (otchet.SposobOpl.ContainsKey("per")) otchet.SposobOpl["per"] += int.Parse(TextBox_SmOpl_per.Text);
                    }
                    otchet.OtchetPoProdazamProductov.Add(new Prodaza(new Tovar(s[0].ToString(), int.Parse(s[1])), sm_opl, dateTime.ToString()));

                }

                ComboBoxProducs.Text = "товар";
                ComboBoxSposobOpl.Text = "способ оплаты";
                TextBox_SmOpl_evo.Text = TextBox_SmOPl_Nal.Text = TextBox_SmOpl_per.Text = "0";

                DataGrid_prodaza.Items.Refresh();
                otchet.UnionProdaz();
                DataGrid_otchet.ItemsSource = otchet.AllProdaz;
                ComboBoxSposobOpl.IsEnabled = false;
                //сортировка по временри ,,,
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Некорректно сделан выбор");
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно сделан выбор");
            }

        }





        private void ComboBoxSposobOpl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSposobOpl.SelectedItem == "Смешанная оплата")
            {
                Grid_SmOpl.Visibility = Visibility.Visible;
            }

        }

        private void Baza_Click(object sender, RoutedEventArgs e)
        {
            Grid_Baza.Visibility = Visibility.Visible;
            Grid_Statistic.Visibility = Visibility.Hidden;
            Grid_Calend.Visibility = Visibility.Hidden;
            Grid_Add_Product.Visibility = Visibility.Hidden;
            Otchets.Visibility = Visibility.Hidden;

            Button_Prodaza.IsEnabled = true;
            Otchet.IsEnabled = true;
            Baza.IsEnabled = false;
            Statistic.IsEnabled = true;
            Data_Grid_Baza_Clientov.ItemsSource = guests;

        }

        private void Button_Add_Client_Click(object sender, RoutedEventArgs e)
        {
     
            new  New_Client().ShowDialog();
            Data_Grid_Baza_Clientov.Items.Refresh();
        }

 
        private void Button_month_Click(object sender, RoutedEventArgs e)
        {
            ComboBox_month.Visibility = Visibility.Visible;
        }




        private void ComboBox_Pos_Selected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedItem = comboBox.SelectedItem.ToString();
            // MessageBox.Show(selectedItem.Content.ToString());
            if (selectedItem == "Залы")
            {
                DataGrid_Statistic_Itogi.Visibility = Visibility.Hidden;
                DataGrid_Statistic_Producs.Visibility = Visibility.Hidden;
                DataGrid_Statistic_Zal.Visibility = Visibility.Visible;

            }
            else if (selectedItem == "Товары")
            {
                DataGrid_Statistic_Itogi.Visibility = Visibility.Hidden;
                DataGrid_Statistic_Zal.Visibility = Visibility.Hidden;
                DataGrid_Statistic_Producs.Visibility = Visibility.Visible;

            }
            else if (selectedItem == "Итоги за день")
            {

                DataGrid_Statistic_Zal.Visibility = Visibility.Hidden;
                DataGrid_Statistic_Producs.Visibility = Visibility.Hidden;
                DataGrid_Statistic_Itogi.Visibility = Visibility.Visible;
            }
        }

        private void Statistic_Click_1(object sender, RoutedEventArgs e)
        {
            Grid_Statistic.Visibility = Visibility.Visible;
            Grid_Baza.Visibility = Visibility.Hidden;
            Grid_Add_Product.Visibility = Visibility.Hidden;
            Grid_Calend.Visibility = Visibility.Hidden;
            Otchets.Visibility = Visibility.Hidden;

            Statistic.IsEnabled = false;
            Baza.IsEnabled = true;
            Button_Prodaza.IsEnabled = true;
            Otchet.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Calendar_Otchet.Visibility = Visibility.Visible;
            Button_DateFrom_Copy.Visibility = Visibility.Visible;
        }

        private void Otchet_Click(object sender, RoutedEventArgs e)
        {
            //
            DataGrid_otchet.ItemsSource = otchet.AllProdaz;
            DataGrid_otchet.Items.Refresh();
            Otchets.Visibility = Visibility.Visible;
            Grid_Statistic.Visibility = Visibility.Hidden;
            Grid_Calend.Visibility = Visibility.Hidden;
            Grid_Baza.Visibility = Visibility.Hidden;
            Grid_Add_Product.Visibility = Visibility.Hidden;

            Otchet.IsEnabled = false;
            Baza.IsEnabled = true;
            Button_Prodaza.IsEnabled = true;
            Statistic.IsEnabled = true;

            
        }

        private void B_del_Click_1(object sender, RoutedEventArgs e)
        {
            if (grid1.SelectedItem != null)
            {
                Preduprezdenie.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("строка с бронированием не выбрана");
            }


        }

        private void ButtonCloseSmOpl_Click(object sender, RoutedEventArgs e)
        {
            TextBox_SmOpl_evo.Text = TextBox_SmOPl_Nal.Text = TextBox_SmOpl_per.Text = "";
            ComboBoxProducs.Text = "товар";
            ComboBoxSposobOpl.Text = "способ оплаты";
            Grid_SmOpl.Visibility = Visibility.Hidden;
            ComboBoxSposobOpl.IsEnabled = false;
        }


        private void Button_AddSmOpl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] s = ComboBoxProducs.SelectedItem.ToString().Split(' ');
                Tovar t = new Tovar(s[0], int.Parse(s[1]));
                if (int.Parse(TextBox_SmOpl_evo.Text) + int.Parse(TextBox_SmOPl_Nal.Text) + int.Parse(TextBox_SmOpl_per.Text) != t.Cost)
                {
                    MessageBox.Show("Не сходится!");
                }
                else
                {
                    Grid_SmOpl.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Неверный формат!");
            }
        }

        private void SelectoonChangeProduct(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxProducs.SelectedIndex.ToString() != "")
            {
                ComboBoxSposobOpl.IsEnabled = true;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Grid_Close_Sm.Visibility = Visibility.Visible;
            TextBox_Sverka_Nal.Text = otchet.SposobOpl["nal"].ToString();
            TextBox_Sverka_Evo.Text = otchet.SposobOpl["evo"].ToString();
            TextBox_Sverka_Per.Text = otchet.SposobOpl["per"].ToString();
            otchet.RasschetViruchki();
            TextBox_Sverka.Text = otchet.Viruchka.ToString();
            //тут всё досчитать и занести, если не сделано

        }

        private void Button_Click_RaschetZp(object sender, RoutedEventArgs e)
        {
            try
            {
                otchet.RasschetZp(int.Parse(TextBox_CountClock.Text), int.Parse(TextBox_RCh.Text), int.Parse(TextBox_Inoe.Text));
                TexBox_ZP.Text = otchet.DayZp.ToString();
            }
            catch
            {
                MessageBox.Show("некорректный ввод");
            }
        }

        private void Button_Click_Close_x(object sender, RoutedEventArgs e)
        {
            Grid_Close_Sm.Visibility = Visibility.Hidden;
            if (TextBox_InoeRashod.Text != "" && TexBox_ZP.Text != "")
                otchet.Viruchka += int.Parse(TextBox_InoeRashod.Text) + int.Parse(TexBox_ZP.Text);
            TexBox_ZP.Text = TextBox_CountClock.Text = TextBox_RCh.Text = TextBox_Inoe.Text = TextBox_InoeRashod.Text = "";

        }

        private void Button_Click_AddZpForViruchka(object sender, RoutedEventArgs e)
        {
            try
            {
                otchet.RasschetViruchki();
                TextBox_Sverka.Text = otchet.Viruchka.ToString();
            }
            catch
            {
                MessageBox.Show("");
            }
        }

        private void Button_AddDrRashod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                otchet.Rashod = int.Parse(TextBox_InoeRashod.Text);
                otchet.RasschetViruchki();
                TextBox_Sverka.Text = otchet.Viruchka.ToString();
            }
            catch 
            {
                MessageBox.Show("невозможное значение");
                TextBox_InoeRashod.Text = "0";
            }
        }

        private void SelectStr(object sender, RoutedEventArgs e)
        {
            B_del.IsEnabled = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Preduprezdenie.Visibility = Visibility.Hidden;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                Preduprezdenie.Visibility = Visibility.Hidden;
                if (grid1.SelectedItem != null)
                {                 
                    Bron bron = grid1.SelectedItem as Bron;
                    Bron emptybron = new Bron { Guest = new Guest { } };
                    emptybron.SetRowGoogle(bron.GetRowGoogle());
                    emptybron.Date = bron.GetDateTime().ToShortDateString();
                    GoogleAPI.SetDataGoogle(service, AddDelChange.Del, ref emptybron);
                    result.Remove(bron);
                    grid1.Items.Refresh();
                }
            }
            catch
            {
                MessageBox.Show("строка с бронированием не выбрана");
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if(TextBoxSerchOfName.Text.Length == 0 && TextBoxSerchOfNumber.Text.Length == 0)
            {
                Data_Grid_Baza_Clientov.ItemsSource = guests;
                return;
            }
            List<Guest> serchClient = new List<Guest>();
            if (TextBoxSerchOfNumber.Text != "" && New_Client.IsAllDigits(TextBoxSerchOfNumber.Text) && TextBoxSerchOfName.Text != "")
            {
             serchClient= guests.FindAll(x => x.Name.Contains(TextBoxSerchOfName.Text.ToUpper()) && x.Phone.Contains(TextBoxSerchOfNumber.Text)).ToList();
            }
            else if (TextBoxSerchOfNumber.Text != "" && New_Client.IsAllDigits(TextBoxSerchOfNumber.Text))
            {
             serchClient=  guests.FindAll(x => x.Phone.Contains(TextBoxSerchOfNumber.Text)).ToList();
            }
            else if (TextBoxSerchOfName.Text != "")
            {
              serchClient=  guests.FindAll(x => x.Name.Contains( TextBoxSerchOfName.Text.ToUpper())).ToList() ;
            }
            Data_Grid_Baza_Clientov.ItemsSource = serchClient;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void B_ras_Click(object sender, RoutedEventArgs e)
        {
            var b = grid1.SelectedItem as Bron;
            if (b is null)
            {
                MessageBox.Show("строка не выбрана");
                return;
            }
            new Rasschet(b).ShowDialog();
            grid1.Items.Refresh();
            DataGrid_otchet.Items.Refresh();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateFilter1 != null)
            {
                DateFilter2 = (DateTime)Calendar.SelectedDate;
                if (DateFilter1 > DateFilter2)
                {
                    DateFilter2 = DateFilter1;
                    DateFilter1 = (DateTime)Calendar.SelectedDate;
                }
                Calendar.Visibility = Visibility.Hidden;
                Button_DateFrom.Content = string.Format("{0} - {1}", DateFilter1.Value.ToShortDateString(), DateFilter2.Value.ToShortDateString());
                result = GoogleAPI.GetDataGoogle(service, DateFilter1.Value, DateFilter2.Value);
                grid1.ItemsSource = result;
                DateFilter1 = DateFilter2 = null;
            }
            else
            {
                DateFilter1 = (DateTime)Calendar.SelectedDate;
                if (DateFilter1 > DateTime.Now) DateFilter1 = DateTime.Now; //а надо ли?
                Button_DateFrom.Content = string.Format("{0} - ", DateFilter1.Value.ToShortDateString());
            }
        }
    }
}
