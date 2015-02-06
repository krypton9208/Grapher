using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Grapher_v2
{
    public class Graph
    {
        public enum Graphs
        {
            Circle,
            Rectangle
        };

        private List<Point> defaultsSquarePoints = new List<Point>();
        public List<Verticle> Verticles = new List<Verticle>();
        public int VerticesNumber { get; set; }
        public double Propability { get; set; }
        public Graphs TypeOfGraph { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public Graph(int vert, double prop, Graphs type, double x, double y)
        {
            VerticesNumber = vert;
            Propability = prop;
            TypeOfGraph = type;
            MaxX = x;
            MaxY = y;
        }

        public Graph(int vert, Graphs type, double x, double y, int[,] relations, int o)
        {
            VerticesNumber = vert;
            Propability = 0;
            TypeOfGraph = type;
            MaxX = x;
            MaxY = y;
            if (TypeOfGraph == Graphs.Circle)
            {
                GraphOnCirle();
            }
            else
            {
                AddDefaultPoints();
                GraphOnRectangle();
            }
            int i = 0;
            if (o == 1)
            {
                foreach (var item in Verticles)
                {

                    for (int j = 0; j < vert; j++)
                    {
                        if (relations[i, j] == 1)
                        {
                            item.AddRelations(j);
                        }
                    }
                    i++;
                }

            }
            else if(o == 0)
            {
                foreach (var item in Verticles)
                {

                    for (int j = 0; j < vert; j++)
                    {
                        if (relations[i, j] == 0)
                        {
                            item.AddRelations(j);
                        }
                    }
                    i++;
                }
            }
            else
            {
                foreach (var item in Verticles)
                {

                    for (int j = 0; j < vert; j++)
                    {
                        
                         item.AddRelations(j);
                       
                    }
                    i++;
                }
            }
            
        }

        private void GraphOnCirle()
        {
            double max = 0;
            if (MaxX >= MaxY) max = MaxY;
            else if (MaxY >= MaxX) max = MaxX;
            double Xtemp, Ytemp;
            double temp;
            Xtemp = (max - 0 - 30) / 2;
            Ytemp = (max - 0 - 30) / 2;

            for (int i = 1; i <= VerticesNumber; i++)
            {
                temp = 2 * (i - 1) * (Math.PI / VerticesNumber);
                Verticle p = new Verticle(Xtemp + 0.95 * Xtemp * Math.Sin(temp) + 15, Ytemp + 0.95 * Ytemp * Math.Cos(temp) + 15);
                Verticles.Add(p);
            }
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

        public int GetRandomNumberInt
        {
            get
            {
                int seed = Guid.NewGuid().GetHashCode();
                Random rand = new Random(seed);
                return rand.Next(VerticesNumber);
            }

        }
        private void GraphOnRectangle()
        {
            double max = 0;
            if (MaxX >= MaxY) max = MaxY;
            else if (MaxY >= MaxX) max = MaxX;
            for (int i = 0; i < VerticesNumber; i++)
            {
                Verticles.Add(new Verticle(defaultsSquarePoints[i].X * max, defaultsSquarePoints[i].Y * max));
            }
        }

        private void GenerateRelations()
        {

            foreach (var item in Verticles)
            {
                for (int i = 0; i < Verticles.Count; i++)
                {
                    if (Propability >= GetRandomNumer)
                    {

                        int t = GetRandomNumberInt;
                        if (t != Verticles.IndexOf(item))
                        {
                            item.AddRelations(t);
                            Verticles[t].AddRelations(Verticles.IndexOf(item));
                        }
                        else if (t == Verticles.IndexOf(item))
                        {
                            do
                            {
                                t = GetRandomNumberInt;
                            } while (t == Verticles.IndexOf(item));
                            item.AddRelations(t);
                            Verticles[t].AddRelations(Verticles.IndexOf(item));
                        }
                    }
                }
            }
        }
        public void Generate()
        {
            if (TypeOfGraph == Graphs.Circle)
            {
                GraphOnCirle();
                GenerateRelations();
            }
            else if (TypeOfGraph == Graphs.Rectangle)
            {
                AddDefaultPoints();
                GraphOnRectangle();
                GenerateRelations();
            }


        }

        private void AddDefaultPoints()
        {
                defaultsSquarePoints.Add(new Point(0.9,0.9));
                defaultsSquarePoints.Add(new Point(0.9, 0.1));
                defaultsSquarePoints.Add(new Point(0.1,0.1));
                defaultsSquarePoints.Add(new Point(0.1,0.9));
                defaultsSquarePoints.Add(new Point(0.5,0.5));
                defaultsSquarePoints.Add(new Point(0.9, 0.5));
                defaultsSquarePoints.Add(new Point(0.1, 0.5));
                defaultsSquarePoints.Add(new Point(0.5, 0.1));
                defaultsSquarePoints.Add(new Point(0.5, 0.9));
                defaultsSquarePoints.Add(new Point(0.75, 0.75));
                defaultsSquarePoints.Add(new Point(0.75, 0.25));
                defaultsSquarePoints.Add(new Point(0.25, 0.25));
                defaultsSquarePoints.Add(new Point(0.25, 0.75));
            
        }


    }
    
}
