using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grapher
{
    class GraphC
    {
        
        public List<Point> punkty = new List<Point>();

        public List<List<int>> relacje = new List<List<int>>();
        public int LiczbaWierzcholkow { get; set; }
        public double PrawdopodobienstwoPolaczen { get; set; }
        public GraphC(int a, double b)
        {
            LiczbaWierzcholkow = a;
            PrawdopodobienstwoPolaczen = b;
        }

        public void GenerateCirclePoints(double Xmax, double Ymax)
        {
            double Xtemp, Ytemp;
            double temp;
            Xtemp = (Xmax - 0 - 30) / 2;
            Ytemp = (Ymax - 0 - 30) / 2;
            
            for (int i = 1; i <= LiczbaWierzcholkow; i++)
            {
                Point p1 = new Point();
                temp = 2 * (i - 1) * (Math.PI / LiczbaWierzcholkow);
                p1.X = Xtemp + 0.95 * Xtemp * Math.Sin(temp) + 15;
                p1.Y = Ytemp + 0.95 * Ytemp * Math.Cos(temp) + 15;
                punkty.Add(p1);
            }
        }
    }
}
