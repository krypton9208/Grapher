using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grapher
{
    class GraphS
    {
        List<Point> punkty = new List<Point>();
        public int LiczbaWierzcholkow { get; set; }
        public int PrawdopodobienstwoPolaczen { get; set; }

        public Point Srodekgrafu { get; set; }
        public GraphS(int a, int b, Point c)
        {
            LiczbaWierzcholkow = a;
            PrawdopodobienstwoPolaczen = b;
        }

        
        //    public static List<Vertex> CrossVertices(int vertexQuantity, int plotSize)
        //{
        //    var vertices = new List<Vertex>();
        //    var ass = (int) Math.Ceiling(Math.Sqrt(vertexQuantity)); // Liczba kolumn
        //    double startPoint;
        //    if (ass < 6)
        //        startPoint = plotSize*(0.20 - ((ass*2)/100.0));
        //    else startPoint = plotSize*(0.20 - ((10)/100.0));
        //    var distance = (plotSize - (startPoint*2))/((double) ass - 1);

        //    var x = startPoint;
        //    var y = startPoint;
        //    var end = 0;
        //    for (var i = 0; i < ass; i++)
        //    {
        //        for (var j = 0; j < ass; j++)
        //        {
        //            vertices.Add(new Vertex(end, x, y));
        //            end++;
        //            if (end == vertexQuantity)
        //                return vertices;
        //            x += distance;
        //        }
        //        y += distance;
        //        x = startPoint;
        //    }

        //    return vertices;
        //}
    }
}
