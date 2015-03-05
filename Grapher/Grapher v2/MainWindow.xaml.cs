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
using System.Windows.Resources;
using System.IO;
using System.Runtime.InteropServices;

using Image = System.Drawing.Image;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Grapher_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        List<Graph> Graphes = new List<Graph>();
        Graph.Graphs SelectedType;
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
        private void CreateLine2(Point a, Point b, Canvas can)
        {

            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = a.X;
            myLine.X2 = b.X;
            myLine.Y1 = a.Y;
            myLine.Y2 = b.Y;
            myLine.HorizontalAlignment = HorizontalAlignment.Center;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            can.Children.Add(myLine);

        }

        private void ReDraw()
        {
            if (Graphes.Count > 0)
            {
                Graphes[0] = new Graph(Graphes[0].VerticesNumber, SelectedType, Canvas1.ActualWidth, Canvas1.ActualHeight, GetTabFromRelations(Graphes[0].Verticles), 1);
                DrawGraph(Canvas1, Graphes[0]);
                Graphes[1] = new Graph(Graphes[0].VerticesNumber, SelectedType, Canvas2.ActualWidth, Canvas2.ActualHeight, GetTabFromRelations(Graphes[0].Verticles), 0);
                DrawGraph(Canvas2, Graphes[1]);
                Graphes[2] = new Graph(Graphes[0].VerticesNumber, SelectedType, Canvas2.ActualWidth, Canvas2.ActualHeight, GetTabFromRelations(Graphes[0].Verticles), 2);
                DrawGraph(Canvas3, Graphes[0]);
                DrawGraph2(Canvas3, Graphes[1]);
            }
        }
        private void DrawGraph(Canvas can, Graph g)
        {
            can.Children.Clear();

            foreach (var item in g.Verticles)
            {
                foreach (var subitem in item.GetRelations)
                {
                    CreateLine(new Point(item.X, item.Y), new Point(g.Verticles[subitem].X, g.Verticles[subitem].Y), can);
                }
            }
            foreach (var item in g.Verticles)
            {
                var ellipse = new Ellipse() { Width = 5, Height = 5, Stroke = new SolidColorBrush(Colors.Red), Fill = new SolidColorBrush(Colors.Red) };
                Canvas.SetLeft(ellipse, item.X - 2.5);
                Canvas.SetTop(ellipse, item.Y - 2.5);
                can.Children.Add(ellipse);
            }
        }

        private void DrawGraph2(Canvas can, Graph g)
        {
         

            foreach (var item in g.Verticles)
            {
                foreach (var subitem in item.GetRelations)
                {
                    CreateLine2(new Point(item.X, item.Y), new Point(g.Verticles[subitem].X, g.Verticles[subitem].Y), can);
                }
            }
            foreach (var item in g.Verticles)
            {
                var ellipse = new Ellipse() { Width = 5, Height = 5, Stroke = new SolidColorBrush(Colors.Red), Fill = new SolidColorBrush(Colors.Red) };
                Canvas.SetLeft(ellipse, item.X - 2.5);
                Canvas.SetTop(ellipse, item.Y - 2.5);
                can.Children.Add(ellipse);
            }
        }

        private int[,] GetTabFromRelations(List<Verticle> list)
        {

            int[,] savetab = new int[list.Count + 1, list.Count + 1];
            int i = 0;

            foreach (var items in list)
            {
                foreach (var item in items.GetRelations)
                {
                    savetab[i, item] = 1;
                }
                i++;
            }
            for (int k = 0; k < list.Count; k++)
            {
                List<int> r = list[k].GetRelations;
                foreach (var item in r)
                {
                    savetab[k, item] = 1;
                }
            }
            return savetab;

        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Graphes.RemoveRange(0, Graphes.Count);
            Graph g1 = new Graph(Convert.ToInt32(VerticesSlider.Value), PropabilitySlider.Value / 100.0, SelectedType, Canvas1.ActualWidth, Canvas1.ActualHeight);
            g1.Generate();
            Graphes.Add(g1);
            Graphes.Add(g1);
            Graphes.Add(g1);
            ReDraw();

        }

        private int[,] GenerateRelationsFromString(string text, int lines)
        {
            int k = 0, y = 0;
            int[,] temp = new int[lines, lines];
            for (int i = 0; i < lines; i++)
            {

                for (int j = 0; j < lines; j++)
                {
                    temp[i, j] = Convert.ToInt32(Convert.ToString(text[((j) * 2) + k]));
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
                int t = Graphes[0].VerticesNumber;
                int[,] savetab = new int[t + 1, t + 1];
                int i = 0;
                List<Verticle> j = Graphes[0].Verticles;
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
                    List<int> r = Graphes[0].Verticles[k].GetRelations;
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
                        File.WriteAllText(@filename, String.Empty);
                        File.AppendAllText(@filename, sb.ToString());
                    }
                    else
                    {
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
                if (RadioButton.IsChecked == true)
                {
                    tempGraphs = Grapher_v2.Graph.Graphs.Circle;
                }
                else
                {
                    tempGraphs = Grapher_v2.Graph.Graphs.Rectangle;
                }
                Graph g1 = new Graph(counter, SelectedType, Canvas1.ActualWidth, Canvas1.ActualHeight, GenerateRelationsFromString(text, counter), 1);
                Graphes.Add(g1);
                Graphes.Add(g1);
                Graphes.Add(g1);
                Graphes[0] = g1;
                ReDraw();
                file.Close();



            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas3.Width = Canvas2.ActualWidth;
            Canvas3.Height = Canvas2.ActualHeight;
            ReDraw();
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            Canvas3.Width = Canvas2.ActualWidth;
            Canvas3.Height = Canvas2.ActualHeight;
            ReDraw();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedType = Graph.Graphs.Circle;
            ReDraw();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            SelectedType = Graph.Graphs.Rectangle;
            ReDraw();
        }

        private void ExportPdf_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument doc = new PdfDocument();

            XImage img1 = XImage.FromBitmapSource(util.SaveCanvas(this, Canvas1, 96));
            img1.Interpolate = false;
            XImage img2 = XImage.FromBitmapSource(util.SaveCanvas(this, Canvas2, 96));
            img2.Interpolate = false;
            XImage img3 = XImage.FromBitmapSource(util.SaveCanvas(this, Canvas3, 96));
            img3.Interpolate = false;

            doc.Pages.Add(new PdfPage());
            XGraphics xgr1 = XGraphics.FromPdfPage(doc.Pages[0]);
            xgr1.DrawImage(img1, 0, 0);

            doc.Pages.Add(new PdfPage());
            XGraphics xgr2 = XGraphics.FromPdfPage(doc.Pages[1]);
            xgr2.DrawImage(img2, 0, 0);

            doc.Pages.Add(new PdfPage());
            XGraphics xgr3 = XGraphics.FromPdfPage(doc.Pages[2]);
            xgr3.DrawImage(img3, 0, 0);


            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf";
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                doc.Save(@dlg.FileName);
                doc.Close();
            }




        }

    }

    public static class util
    {
        public static void SaveWindow(Window window, int dpi, string filename)
        {

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width 
                (int)window.Width, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(window);

            SaveRTBAsPNG(rtb, filename);

        }

        public static RenderTargetBitmap SaveCanvas(Window window, Canvas canvas, int dpi)
        {
            Size size = new Size(window.Width, window.Width);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)canvas.ActualWidth, //width 
                (int)canvas.ActualWidth, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(canvas);

            return rtb;
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}
