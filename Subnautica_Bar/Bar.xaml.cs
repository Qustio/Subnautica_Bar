using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Subnautica_Bar
{
    public partial class Bar : Window
    {
        private double Per = 1;
        private bool StillWorking = true;
        private bool ShowCPU = true;
        const double Thikness = 28;
        const double R = 105 - Thikness;
        const double Delta = 0.02;

        private const double PI = 3.141592;

        public Bar()
        {
            InitializeComponent();
            Arc.Size = new Size(R, R);
            GetCPULoadAsync();
        }

        private async void GetCPULoadAsync()
        {
            PerformanceCounter cpuCounter = new ("Processor", "% Idle Time", "_Total");
            //double first = cpuCounter.NextValue();
            while (StillWorking)
            {
                if (ShowCPU)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Update((100 - cpuCounter.NextValue()) / 100);
                    });
                }
                await Task.Delay(1000);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MouseLeftButtonDown += delegate { DragMove(); };
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!ShowCPU)
            {
                if (e.Delta > 0 && Per <= 1)
                {
                    Update(Per + Delta);
                }
                else if (e.Delta < 0 && Per >= 0)
                {
                    Update(Per - Delta);
                }
            }
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            Update(0.5);
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            if (ShowCPU)
            {
                ShowCPU = false;
                Update(0.5);
                ResetButton.IsEnabled = true;
                ManualButton.Header = "CPU mode";
            }
            else
            {
                ShowCPU = true;
                ResetButton.IsEnabled = false;
                ManualButton.Header = "Manual mode";
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            StillWorking = false;
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        

        private Point CalcPoint(double A, double Offset)
        {
            Point B = new Point();
            B.Y = R * -Math.Cos(PI * 2 * A) + R + Offset;
            B.X = R * Math.Sin(PI * 2 * A) + R + Offset;
            return B;
        }

        private void Update(double NPer)
        {
            Point End = CalcPoint(NPer, Thikness);
            if (NPer > 0.5)
            {
                Arc.IsLargeArc = true;
            }
            else
            {
                Arc.IsLargeArc = false;
            }
            Arc.Point = End;
            PercentageTextBlock.Text = Convert.ToString(Convert.ToInt32(100 * NPer) + "%");
            Per = NPer;
        }
    }
}