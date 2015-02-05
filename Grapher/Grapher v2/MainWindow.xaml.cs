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
using System.IO;
using System.Runtime.InteropServices;

namespace Grapher_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        List<Graph> Graphes = new List<Graph>(); 
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CreateLine(Point a, Point b, Canvas can)
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
            can.Children.Add(myLine);

        }
        private void DrawGraph(Canvas can, Graph g)
        {
            can.Children.Clear();
            
            foreach (var item in g.Verticles)
            {
                foreach (var subitem in item.GetRelations)
                {
                   // MessageBox.Show(Convert.ToString(subitem));
                    CreateLine(new Point(item.X, item.Y),new Point(g.Verticles[subitem].X, g.Verticles[subitem].Y), can );
                }
            }
            foreach (var item in g.Verticles)
            {
                var ellipse = new Ellipse() { Width = 5, Height = 5, Stroke = new SolidColorBrush(Colors.Red), Fill = new SolidColorBrush(Colors.Red)};
                Canvas.SetLeft(ellipse, item.X - 2.5);
                Canvas.SetTop(ellipse, item.Y - 2.5);
                can.Children.Add(ellipse);
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Graphes.RemoveRange(0, Graphes.Count);
            Graph g1 = new Graph(Convert.ToInt32(VerticesSlider.Value), PropabilitySlider.Value / 100.0, Graph.Graphs.Rectangle, Canvas1.ActualWidth, Canvas1.ActualHeight);
            g1.Generate();
            Graphes.Add(g1);
            DrawGraph(Canvas1, g1);

        }

        private int[,] GenerateRelationsFromString(string text, int lines)
        {
            int k = 0, y=0;
            int[,] temp = new int[lines, lines];
            for (int i = 0; i < lines; i++)
            {
               
                for (int j = 0; j < lines; j++)
                {
                    temp[i, j] = Convert.ToInt32(Convert.ToString(text[((j)*2)+k]));
                }
                y++;
                k = 2 * lines * y;
            }
            return temp;

        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Graphes.Count > 0)
            {
                int t = Graphes[Graphes.Count - 1].VerticesNumber;
                int[,] savetab = new int[t + 1, t + 1];
                int i = 0;
                List<Verticle> j = Graphes[Graphes.Count - 1].Verticles;
                foreach (var items in j)
                {
                    foreach (var item in items.GetRelations)
                    {
                        savetab[i, item] = 1;
                    }
                    i++;
                }
                for (int k = 0; k < t; k++)
                {
                    List<int> r = Graphes[Graphes.Count - 1].Verticles[k].GetRelations;
                    foreach (var item in r)
                    {
                        savetab[k, item] = 1;
                    }
                }

                var sb = new StringBuilder();
                for (int k = 0; k < t; k++)
                {
                    string p = "";
                    for (int l = 0; l < t; l++)
                    {
                        if (savetab[k, l] == 1) p += "1;";
                        else p += "0;";
                    }
                    sb.AppendLine(p);
                }

                File.WriteAllText(@"D:\x.txt", sb.ToString());
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text Files (*.txt)|*.txt";
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    string filename = dlg.FileName;
                    if (File.Exists(@filename))
                    {
                        MessageBox.Show("yes");
                        File.WriteAllText(@filename, String.Empty);
                        File.AppendAllText(@filename, sb.ToString());
                    }
                    else
                    {
                        MessageBox.Show("no");
                        File.Create(@filename).Dispose();
                        File.WriteAllText(@filename, sb.ToString());
                    }
                }

            }
            else MessageBox.Show("Utwórz graf najpierw :)");
            
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;

                int counter = 0;
                string line = "";
                string text = "";

                System.IO.StreamReader file =
                   new System.IO.StreamReader(@filename);
                while ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    text += line;
                    counter++;
                }
                Graph.Graphs tempGraphs;
                if(RadioButton.IsChecked == true)
                {
                    tempGraphs = Grapher_v2.Graph.Graphs.Circle;
                }
                else
                {
                    tempGraphs = Grapher_v2.Graph.Graphs.Rectangle;
                }
                Graph g1 = new Graph(counter, Graph.Graphs.Circle, Canvas1.ActualWidth, Canvas1.ActualHeight,GenerateRelationsFromString(text, counter) );
                Graphes.Add(g1);
                DrawGraph(Canvas1, g1);
                Graph g2 = new Graph(counter, Graph.Graphs.Rectangle, Canvas3.ActualWidth, Canvas3.ActualHeight, GenerateRelationsFromString(text, counter));
                Graphes.Add(g2);
                DrawGraph(Canvas3, g2);
                
                
                file.Close();

               

            }
        }
       
    }
}
