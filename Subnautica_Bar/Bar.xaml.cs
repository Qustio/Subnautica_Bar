using System;
using System.Windows;
using System.Windows.Input;

namespace Subnautica_Bar
{
    public partial class Bar : Window
    {
        public double Per = 1;
        const double Thikness = 28;
        const double R = 105 - Thikness;
        const double Delta = 0.02;

        private const double PI = 3.141592;

        public Bar()
        {
            InitializeComponent();
            Arc.Size = new Size(R, R);
            Update(Per);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MouseLeftButtonDown += delegate { DragMove(); };
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && Per < 1)
            {
                Update(Per + Delta);
            }
            else if (e.Delta < 0 && Per > 0)
            {
                Update(Per - Delta);
            }

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            Update(1);
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