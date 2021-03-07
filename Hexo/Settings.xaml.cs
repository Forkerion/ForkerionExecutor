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

namespace Forkerion
{
    /// <summary>
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void SMBtn_Click(object sender, RoutedEventArgs e)
        {
            SMTB.Text = $"loadstring(game:HttpGet(\"{SMTB.Text}\", true))()";
            
        }
        public static bool topMost;
        public void AutoAttachCB_Click(object sender, RoutedEventArgs e)
        {
            if(AutoAttachCB.IsChecked == true)
            {
                topMost = true;
            }
            else
            {
                topMost = false;
            }
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }

        private void MinimizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}