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
    /// Логика взаимодействия для New_Client.xaml
    /// </summary>
    public partial class New_Client : Window
    {
        public New_Client(Guest guest)
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                ComboBox_CountVizit.Items.Add(i+1);
            }
           // if(TextBox_NameCliient.Text!="")
          
        }

        public void Button_Click()
        {
            //доб  в бузу клиентов,сорт

          




        }

        private void ButtonAddNewClient_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();

        }
    }
}
