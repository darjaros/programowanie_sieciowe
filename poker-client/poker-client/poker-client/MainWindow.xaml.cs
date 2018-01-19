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

namespace poker_client
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int Cache = 10000;
        

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider.Value = (int)slider.Value;
        }

        private void Rate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                int test = Int32.Parse(Rate.Text);
            }
            catch {
               
                MessageBox.Show("Podaj liczbe całkowitą");
            }
        }
    }
}
