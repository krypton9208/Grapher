using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grapher_v2
{
    public class Verticle
    {
        private List<int> _relations = new List<int>(); 
        public double Y { get; set; }
        public double X { get; set; }
        public Verticle(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void AddRelations(int n)
        {
            _relations.Add(n);
        }

        public Point GetPosition
        {
            get
            {
                return new Point(X,Y);
            }
        }

        public List<int> GetRelations
        {
            get { return _relations; }
        }

        public void DeleteRelations(int n)
        {
            _relations.RemoveAt(n);
        }
    }
}
