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
        public New_bron()
        {
            InitializeComponent();
            ComboBox_Zal.Items.Add("Большой");
            ComboBox_Zal.Items.Add("Средний");
            ComboBox_Zal.Items.Add("Малый");

            for (int i = 1; i <= 20; i++)
            {
                ComboBox_countPerson.Items.Add(i);
            }
            ComboBox_countPerson.Items.Add("20+");
        }

        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
