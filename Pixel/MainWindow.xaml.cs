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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;

namespace Pixel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Admin> admins= new List<Admin>();
            var xml = new XmlSerializer(typeof(List<Admin>));
            admins = (List<Admin>)xml.Deserialize(new StreamReader("AdminList.xml"));
            foreach (Admin admin in admins)
                ComboBox_Login.Items.Add(admin);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Login.SelectedItem == null/* || логин и пароль неверные*/)
            {
                MessageBox.Show("пока");
                return;
            }
            new General(ComboBox_Login.SelectedItem.ToString()).ShowDialog();
            StartWindow.Close();
        }
    }
}
