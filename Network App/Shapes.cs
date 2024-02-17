using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_11_22_oop2
{
    public class Shapes
    {
        // data -- class variables -- attributes -- class fields
        public float _length, _width;

        // operations -- methods -- functions
        public float getArea() => _length * _width;
        public float getPerimeter() => 2 * (_length + _width);
    }

    public class Circle
    {
        public float _radius;

        public double getArea() => Math.PI * Math.Pow(_radius, 2);
    }
}
