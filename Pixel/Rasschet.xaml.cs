﻿using System;
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
            box_hr.Text = bron.CountofHrs().ToString();
            box_po.Text = bron.PredOpl;
        }

       
        void GetNewSum()
        {
            try
            {
                float sum = (int.Parse(box_po.Text) + (float.Parse(box_hr.Text) * int.Parse(box_r_hr.Text))) * (100 - int.Parse(box_skidka.Text)) / 100
                    - int.Parse(box_fine.Text);
                TextBox_Sum.Text = sum.ToString();
            }
            catch { TextBox_Sum.Text = ""; }
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
            var xml = new XmlSerializer(typeof(List<Bron>));
            var list = (List<Bron>)xml.Deserialize(new StreamReader("archive.xml"));
            list.Add(bron);
            xml.Serialize(new StreamWriter("archive.xml"), list);

            General.otchet.OtchetPoProdazamZalov.Add(new Prodaza(new Tovar(bron.Zal.ToString(), int.Parse(TextBox_Sum.Text)), ComboBoxSposobOpl.Text,
                DateTime.Now.ToString()));

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
            
        }
    }
}
