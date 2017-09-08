using NUnit.Framework;
using UnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Tests
{
    [TestFixture()]
    public class OperationTests
    {
        [Test()]
        public void AddTest()
        {
            Operation op = new Operation();
            Assert.AreEqual(5, op.Add(2, 3));
        }

        [Test()]
        public void MinusTest()
        {
            Operation op = new Operation();
            Assert.AreEqual(5, op.Minus(7, 2));
        }
    }
}