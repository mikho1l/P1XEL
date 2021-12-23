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
using System.Xml.Serialization;
using System.IO;

namespace Pixel
{
    /// <summary>
    /// Логика взаимодействия для Rasschet.xaml
    /// </summary>
    public partial class Rasschet : Window
    {
        Bron bron;
        public Rasschet(Bron b)
        {
            InitializeComponent();
            bron = b;
            Box_date.Text = bron.GetDateTime().Date.ToShortDateString();
            TexBox_time_p.Text = bron.StartTime;
            if (bron.EndTime.Length != 0)
                TexBox_Time_u.Text = bron.EndTime;
            Box_guestcount.Text = bron.GuestCount;
            Box_zal.Text = bron.Zal.ToString();
            TexBox_Comment.Text = bron.Comment;
            TexBox_Name.Text = bron.Guest.Name;
            TexBox_Number.Text = bron.Guest.Phone;
            try
            {
                box_hr.Text = bron.CountofHrs().ToString();
            }
            catch { }
            box_po.Text = bron.PredOpl;
        }

       
        void GetNewSum()
        {
            try
            {
                float sum = (int.Parse(box_fine.Text) + (float.Parse(box_hr.Text) * int.Parse(box_r_hr.Text))) * (100 - int.Parse(box_skidka.Text)) / 100
                    - int.Parse(box_po.Text);
                TextBox_Sum.Text = sum.ToString();
            }
            catch { }
        }

        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bron.Date = Box_date.Text;
                bron.StartTime = TexBox_time_p.Text;
                bron.EndTime = TexBox_Time_u.Text;
                bron.GuestCount = Box_guestcount.Text;
                bron.Guest.Name = TexBox_Name.Text;
                bron.Guest.Phone = TexBox_Number.Text;
            }
            catch
            {
                MessageBox.Show("Неверный формат");
                return;
            }
            int Sum;
            if (!int.TryParse(TextBox_Sum.Text,out Sum))
            {
                MessageBox.Show("Не все данные введены корректно");
                return;
            }
            var xml = new XmlSerializer(typeof(List<Bron>));
            var sr = new StreamReader("archive.xml");
            var list = (List<Bron>)xml.Deserialize(sr);
            sr.Close();
            list.Add(bron);
            var sw = new StreamWriter("archive.xml");
            xml.Serialize(sw, list);
            sw.Close();
            General.result.Remove(bron);
             Tovar t = new Tovar(bron.Zal.ToString(), Sum);
            var d = ComboBoxSposobOpl.Text;
            if (General.otchet.SposobOpl.ContainsKey("evo") && d == "Эвотор") General.otchet.SposobOpl["evo"] += t.Cost;
            if (General.otchet.SposobOpl.ContainsKey("nal") && d == "Наличные") General.otchet.SposobOpl["nal"] += t.Cost;
            if (General.otchet.SposobOpl.ContainsKey("per") && d == "Перевод") General.otchet.SposobOpl["per"] += t.Cost;
            General.otchet.OtchetPoProdazamZalov.Add(new Prodaza(t, ComboBoxSposobOpl.Text,
                DateTime.Now.ToString()));
            General.otchet.UnionProdaz();

            Close();
        }

        private void box_hr_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetNewSum();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetNewSum();
        }

        private void box_po_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetNewSum();
        }

        private void box_skidka_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetNewSum();
        }

        private void box_fine_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetNewSum();
        }

        private void TexBox_Time_u_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                bron.EndTime = TexBox_Time_u.Text;
                box_hr.Text = bron.CountofHrs().ToString();
            }
            catch
            {
                MessageBox.Show("Неверный формат времени ухода");
            }
        }

        private void TexBox_time_p_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                bron.StartTime = TexBox_time_p.Text;
                box_hr.Text = bron.CountofHrs().ToString();
            }
            catch
            {
                MessageBox.Show("Неверный формат времени ухода");
            }
        }
    }
}
