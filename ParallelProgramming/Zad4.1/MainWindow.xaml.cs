using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Zad4._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread _progressThread;
        private Thread _workerThread;
        private static int _percentOfProgress;
        private int TotalSteps;
        private bool _finished;
        
        public MainWindow()
        {
            InitializeComponent();
            _progressThread = new Thread(UpdateProgress);
            _workerThread = new Thread(MakeCalculations);
            _percentOfProgress = 0;
        }

        private void MakeCalculations()
        {
            Dispatcher.Invoke(() => StartButton.IsEnabled = false);
            Dispatcher.Invoke(() => ResultLabel.Content = "Start obliczeń");
            for (int i = 1; i <= TotalSteps; i++)
            {
                
                _percentOfProgress = i;
                Thread.Sleep(10);
            }
            _finished = true;
            Dispatcher.Invoke(() => StartButton.IsEnabled = true);
            Dispatcher.Invoke(() => ResultLabel.Content = "Wykonano obliczenia") ;
        }

        private void UpdateProgress()
        {
            try
            {
                while (_percentOfProgress <= TotalSteps)
                {
                    Dispatcher.Invoke(() => ProgressBar.Value = _percentOfProgress);
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ProgressBar.Maximum = Convert.ToDouble(textBox.Text);
                TotalSteps = Convert.ToInt32(textBox.Text);
                _progressThread.Start();
                _workerThread.Start();
            }
            catch (FormatException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd");
            }
            
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_progressThread.IsAlive && _workerThread.IsAlive)
            {
                TotalSteps = _percentOfProgress;
                ProgressBar.Value = 0;
                ResultLabel.Content = "Przerwano obliczenia";

            }

        }
    }
}
