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

namespace Pixel
{
    /// <summary>
    /// Логика взаимодействия для General.xaml
    /// </summary>
    public partial class General : Window
    {
        List<Bron> result = new List<Bron>();
        DateTime? DateFilter1, DateFilter2 = null;

        public List<Guest> guests = new List<Guest>();
        

        List<Tovar> tovars = new List<Tovar>();
        Google.Apis.Auth.OAuth2.UserCredential credential;
        Google.Apis.Sheets.v4.SheetsService service;
        public General()
        {
            InitializeComponent();
            //кнопки
            Grid_Add_Product.Visibility = Visibility.Hidden;
       
            ComboBox_month.Text = "период";
            ComboBoxSposobOpl.Items.Add("Наличные");
            ComboBoxSposobOpl.Items.Add("Эвотор");
            ComboBoxSposobOpl.Items.Add("Перевод");
            ComboBoxSposobOpl.Items.Add("Смешанная оплата");

            ComboBox_month.Items.Add("все время");
            ComboBox_month.Items.Add("Январь");
            ComboBox_month.Items.Add("Февраль");
            ComboBox_month.Items.Add("Март");
            ComboBox_month.Items.Add("Апрель");
            ComboBox_month.Items.Add("Май");
            ComboBox_month.Items.Add("Июнь");
            ComboBox_month.Items.Add("Июль");
            ComboBox_month.Items.Add("Август");
            ComboBox_month.Items.Add("Сентябрь");
            ComboBox_month.Items.Add("Октябрь");
            ComboBox_month.Items.Add("Ноябрь");
            ComboBox_month.Items.Add("Деабрь");

            ComboBox_pos.Items.Add("Залы");
            ComboBox_pos.Items.Add("Товары");
            ComboBox_pos.Items.Add("Итоги за день");

            //Govno для проверки
            ComboBoxProducs.Items.Add("пиво 100");

            var now = DateTime.Now;
            var nowplus6 = now.AddDays(6);
            Button_DateFrom.Content = string.Format("{0} - {1}", now.ToShortDateString(), nowplus6.ToShortDateString());
            credential = GoogleAPI.GetUserCredentials();
            service = GoogleAPI.GetService(credential);
            result.AddRange(GoogleAPI.GetDataGoogle(service, now, nowplus6));
            grid1.ItemsSource = result;
            Data_Grid_Baza_Clientov.ItemsSource = guests;
            DataGrid_prodaza.ItemsSource = tovars;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Prodaza.IsEnabled=true;
            Baza.IsEnabled = true;
            Statistic.IsEnabled= true;
            Otchet.IsEnabled=true;

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
            Otchet.IsEnabled=true;
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

        private void grid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //grid1.Items.Refresh();
        }

        private void B_del_Click(object sender, RoutedEventArgs e)
        {

        }

        private void B_new_b_Click_1(object sender, RoutedEventArgs e)
        {
            new New_bron().Show();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxSposobOpl.SelectedItem == null) { MessageBox.Show("Некорректно сделан выбор"); return; }
                string[] s = ComboBoxProducs.SelectedItem.ToString().Split(' ');


                if (ComboBoxSposobOpl.Text.ToString() != "Смешанная оплата")
                {
                    //tovars.Add(new Tovar(s[0].ToString(), int.Parse(s[1]), ComboBoxSposobOpl.Text.ToString()));

                }
                else
                {
                    string sm_opl = "смешанная оплата ";
                    if (TextBox_SmOpl_evo.Text != "0") sm_opl += TextBox_SmOpl_evo.Text + " эвотор ";
                    if (TextBox_SmOPl_Nal.Text != "0") sm_opl += TextBox_SmOPl_Nal.Text + " наличными ";
                    if (TextBox_SmOpl_per.Text != "0") sm_opl += TextBox_SmOpl_per.Text + " переводом";
                    sm_opl += " =" + s[1];
                    //tovars.Add(new Tovar(s[0].ToString(), int.Parse(s[1]), sm_opl));

                }
                ComboBoxProducs.Text = "товар";
                ComboBoxSposobOpl.Text = "способ оплаты";
                TextBox_SmOpl_evo.Text = TextBox_SmOPl_Nal.Text = TextBox_SmOpl_per.Text = "0";

                DataGrid_prodaza.Items.Refresh();

                //сортировка по временри ,,,
            }
            catch (System.NullReferenceException n)
            {
                MessageBox.Show("Некорректно сделан выбор");
            }
            catch(Exception )
            {
                MessageBox.Show("Некорректно сделан выбор");
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
            Otchet.IsEnabled=true;
            Baza.IsEnabled = false;
            Statistic.IsEnabled = true;
           

        }

        private void Button_Add_Client_Click(object sender, RoutedEventArgs e)
        {
            Guest guest=new Guest();
            new New_Client(guest).ShowDialog(); // вызываем окно, передавая данные
                                                // guests.Add(New_Client.Button_Click();
            guests.Add(guest);
            Data_Grid_Baza_Clientov.Items.Refresh();
        }

        private void Button_stat_on_zal_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_month_Click(object sender, RoutedEventArgs e)
        {
            ComboBox_month.Visibility = Visibility.Visible;
        }

       

        private void button_all_time_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_Select(object sender, RoutedEventArgs e)
        {
           //w if()
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
            Grid_Calend.Visibility=Visibility.Hidden;
            Otchets.Visibility=Visibility.Hidden;

            Statistic.IsEnabled = false;
            Baza.IsEnabled = true;
            Button_Prodaza.IsEnabled = true;
            Otchet.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Calendar_Otchet.Visibility= Visibility.Visible;
            Button_DateFrom_Copy.Visibility= Visibility.Visible;
        }

        private void Otchet_Click(object sender, RoutedEventArgs e)
        {
            //
            Otchets.Visibility = Visibility.Visible;
            Grid_Statistic.Visibility = Visibility.Hidden;
            Grid_Calend.Visibility = Visibility.Hidden;
            Grid_Baza.Visibility= Visibility.Hidden;
            Grid_Add_Product.Visibility=Visibility.Hidden;
            
            Otchet.IsEnabled = false;
            Baza.IsEnabled = true;
            Button_Prodaza.IsEnabled = true;
            Statistic.IsEnabled = true;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Grid_Close_Sm.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Grid_Close_Sm.Visibility=Visibility.Hidden;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Grid_Close_Sm.Visibility = Visibility.Visible;
        }

        private void TextBox_CountClock_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
           
        }

        private void SelectionChangedCount(object sender, RoutedEventArgs e)
        {
            
         //   TexBox_ZP.Text = (int.Parse(TextBox_CountClock.Text) * int.Parse(TextBox_RCh.Text) + int.Parse(TextBox_Inoe.Text)).ToString();
        }

        private void Button_Click_Raschet(object sender, RoutedEventArgs e)
        {
            if (TextBox_CountClock.Text != "" && TextBox_RCh.Text != "")
            {
                if (TextBox_Inoe.Text != "")
                    TexBox_ZP.Text = (int.Parse(TextBox_CountClock.Text) * int.Parse(TextBox_RCh.Text) + int.Parse(TextBox_Inoe.Text)).ToString();
                else
                    TexBox_ZP.Text = (int.Parse(TextBox_CountClock.Text) * int.Parse(TextBox_RCh.Text)).ToString();
            }
            else 
            {
                MessageBox.Show("Некорректно введены данные");
            }
        }

        private void ButtonCloseSmOpl_Click(object sender, RoutedEventArgs e)
        {
            Grid_Close_Sm.Visibility = Visibility.Hidden;
            TextBox_SmOpl_evo.Text = TextBox_SmOPl_Nal.Text = TextBox_SmOpl_per.Text = "0";
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
           
        }

      
      

        private void SelectionSmOpl(object sender, SelectionChangedEventArgs e)
        {

            if (ComboBoxSposobOpl.SelectedItem == "Смешанная оплата")
            {
                Grid_SmOpl.Visibility = Visibility.Visible;
            }
             return;
        }

        private void Button_Click_Add_SmOPl(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] s = ComboBoxProducs.SelectedItem.ToString().Split(' ');
                if (int.Parse(TextBox_SmOpl_evo.Text) + int.Parse(TextBox_SmOPl_Nal.Text) + int.Parse(TextBox_SmOpl_per.Text) != int.Parse(s[1]))
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

        private void B_ras_Click(object sender, RoutedEventArgs e)
        {

        }

        private void B_del_Click_1(object sender, RoutedEventArgs e)
        {
            Bron bron = grid1.SelectedItem as Bron;
            Bron emptybron = new Bron { Guest = new Guest { } }; 
            emptybron.SetRowGoogle(bron.GetRowGoogle());
            emptybron.Date = bron.GetDateTime().ToShortDateString();
            GoogleAPI.SetDataGoogle(service, AddDelChange.Del, ref emptybron);
            result.Remove(bron);
            grid1.Items.Refresh();
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
