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
    /// Логика взаимодействия для New_bron.xaml
    /// </summary>
    public partial class New_bron : Window
    {
        Bron bron;
        public New_bron(Bron b)
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

        }

        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bron.Date = Box_date.Text;
                bron.StartTime = TexBox_time_p.Text;
                if (TexBox_Time_u.Text.Length != 0)
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
            GoogleAPI.SetDataGoogle(General.service, AddDelChange.Change,ref bron);
            this.Close();
        }
    }
}
