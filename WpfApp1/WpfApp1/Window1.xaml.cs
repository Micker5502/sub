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
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        Properties.Settings setting = new WpfApp1.Properties.Settings();
        public Window1()
        {
            InitializeComponent();
            channel_TB.Text = setting.Channel_ID;
            start_up.IsChecked = setting.startup_Check;
            back_TB.Text = setting.back_GroupPic;
            logo_TB.Text = setting.logo_Pic;
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            setting.startup_Check = true;
        }

        private void start_up_Unchecked(object sender, RoutedEventArgs e)
        {
            setting.startup_Check = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            setting.Channel_ID = channel_TB.Text;
            if (setting.startup_Check)
            {
                rkApp.SetValue("SubCount", System.Windows.Forms.Application.ExecutablePath.ToString());
            }
            else
            {
                rkApp.DeleteValue("SubCount", false);
            }

            setting.Save();
            this.Close();
        }




        private void back_TB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                setting.back_GroupPic = op.FileName;
                back_TB.Text = op.FileName;
            }
        }

        private void logo_TB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.gif|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                setting.logo_Pic = op.FileName;
                logo_TB.Text = op.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                setting.back_GroupPic = op.FileName;
                back_TB.Text = op.FileName;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.gif|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                setting.logo_Pic = op.FileName;
                logo_TB.Text = op.FileName;
            }
        }
    }
}
