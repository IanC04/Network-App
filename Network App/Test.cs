using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_11_22_oop2
{
    public class Calculator
    {
        // data portion
        public int _x, _y;

        // operations
        public int addInt() => _x + _y;
        public int subtractInt() => _x - _y;
        public int multitplyInt() => _x * _y;
        public int divideInt() => _y == 0 ? 0 : _x / _y;
    }
}

