using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class Operation
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Minus(int a, int b)
        {
            return a - b;
        }

        public int Divide(int a, int b)
        {
            return a / b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }
    }
}
