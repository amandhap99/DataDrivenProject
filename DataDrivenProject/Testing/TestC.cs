using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataDrivenProject.Testing
{
    [TestFixture]
    [Parallelizable]
    public class TestC
    {
        [Test]
        public void testC()
        {
            Console.WriteLine("Test C start");
            Thread.Sleep(2000);
            Console.WriteLine("Test C end");
        }
    }
}
