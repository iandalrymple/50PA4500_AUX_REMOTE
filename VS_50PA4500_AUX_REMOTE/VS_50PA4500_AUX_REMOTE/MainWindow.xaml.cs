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
using System.IO.Ports;
using System.Threading;

namespace VS_50PA4500_AUX_REMOTE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Listener listener;

        public MainWindow()
        {
            InitializeComponent();
            listener = null;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate object on listener 
            Listener listener = new Listener();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // Stop the thread if it is running
            listener.StopFlag = true;
            
            // Wait for the thread to be done 
            while(listener.isThreadRunning())
            {

            }
        }
    }
}
