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
using ReadWriteCsv;
using System.Threading;
using System.IO;

namespace Grapher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml       
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        List<List<int>> generated = new List<List<int>>(); 
        public MainWindow()
        {
            InitializeComponent();
        }

        public double GetRandomNumer
        {
            get
            {
                int seed = Guid.NewGuid().GetHashCode();
                Random rand = new Random(seed);
                return rand.NextDouble();
            }
           
        }
        public void DrawGraph()
        {
            GraphCanvas.Children.Clear();
            GraphC g = new GraphC(Convert.ToInt32(Slider1.Value), Convert.ToInt32(Slider2.Value));
            
            g.GenerateCirclePoints(GraphCanvas.ActualWidth, GraphCanvas.ActualHeight);
            foreach (Point point in g.punkty)
            {
                var ellipse = new Ellipse() { Width = 5, Height = 5, Stroke = new SolidColorBrush(Colors.Red) };
                Canvas.SetLeft(ellipse, point.X);
                Canvas.SetTop(ellipse, point.Y);
                GraphCanvas.Children.Add(ellipse);

            }
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < g.punkty.Count; i++)
            {
                for (int j = 0; j < g.punkty.Count; j++)
                {
                    
                    
                    double r = rnd.NextDouble();
                    if (g.PrawdopodobienstwoPolaczen >= (r))
                    {
                        CreateLine(g.punkty[i], g.punkty[j]);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void CreateLine(Point a, Point b)
        {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Black;
            myLine.X1 = a.X;
            myLine.X2 = b.X;
            myLine.Y1 = a.Y;
            myLine.Y2 = b.Y;
            myLine.HorizontalAlignment = HorizontalAlignment.Center;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            GraphCanvas.Children.Add(myLine);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GraphCanvas.Children.Clear();
            GraphC g = new GraphC(Convert.ToInt32(Slider1.Value), Slider2.Value/100);

            g.GenerateCirclePoints(GraphCanvas.ActualWidth, GraphCanvas.ActualHeight);
            int t = 1;
            foreach (Point point in g.punkty)
            {
                var ellipse = new Ellipse() { Width = 30, Height = 30, Stroke = new SolidColorBrush(Colors.Red), Fill = new SolidColorBrush(Colors.Red) ,Tag = t.ToString() };
                //ellipse.Tag
                Canvas.SetLeft(ellipse, point.X - 15);
                Canvas.SetTop(ellipse, point.Y - 15);
                
                GraphCanvas.Children.Add(ellipse);
                t++;

            }
            //generated.AddRange(generated.AddRange(g.punkty.Count));
            for (int i = 0; i < g.punkty.Count; i++)
            {
                
                for (int j = 0; j < g.punkty.Count; j++)
                {
                    
                    if ((g.PrawdopodobienstwoPolaczen > GetRandomNumer) && (i!=j ))
                    {
                        CreateLine(g.punkty[i], g.punkty[j]);
                        //generated[i].Add(1);
                    }
                    //generated[i].Add(0);
                }
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                GraphC w = new GraphC(LoadGraph(filename).Count, 0);
                w.relacje = LoadGraph(filename);
                w.GenerateCirclePoints(GraphCanvas.ActualWidth, GraphCanvas.ActualHeight);
                foreach (Point point in w.punkty)
                {
                    var ellipse = new Ellipse() { Width = 30, Height = 30, Stroke = new SolidColorBrush(Colors.Red), Fill = new SolidColorBrush(Colors.Red) };
                    Canvas.SetLeft(ellipse, point.X - 15);
                    Canvas.SetTop(ellipse, point.Y - 15);
                    GraphCanvas.Children.Add(ellipse);

                }
                DrawLines(w);
            }
            
           
        }

        private void DrawLines(GraphC w)
        {
            for (int i = 0; i < w.punkty.Count; i++)
            {
                for (int j = 0; j < w.punkty.Count; j++)
                {
                    if ((w.relacje[i][j] == 1) && (i != j) && (w.relacje[i][j] == w.relacje[j][i]))
                    {
                        CreateLine(w.punkty[i], w.punkty[j]);
                    }
                }
            }
        }

        private static List<List<int>> LoadGraph( string name)
        {
            List<List<int>> t = new List<List<int>>();
            using (CsvFileReader reader = new CsvFileReader(name))
            {
                CsvRow row = new CsvRow();

                while (reader.ReadRow(row))
                {
                    List<int> sublist = new List<int>();
                    foreach (string s in row)
                    {
                        foreach (var item in s)
                        {
                            switch (item)
                            {
                                case '0':
                                    {
                                        sublist.Add(0);
                                        break;
                                    }
                                case '1':
                                    {
                                        sublist.Add(1);
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                    }
                    t.Add(sublist);
                }


            }
            return t;
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                string csv = String.Join(",", generated.Select(x => x.ToString()).ToArray());
                using (StreamWriter outfile = new StreamWriter(filename, true))
                {
                    await outfile.WriteAsync(csv.ToString());
                }
            }
        }

    }
}
