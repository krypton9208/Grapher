using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        private void GenerateRelations()
        {

            foreach (var item in Verticles)
            {
                for (int i = 0; i < Verticles.Count ; i++)
                {
                    if (Propability >= GetRandomNumer )
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
                GraphOnRectangle();
                GenerateRelations();
            }


        }


    }
}
