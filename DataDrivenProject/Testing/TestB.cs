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
    public class TestB
    {
        [Test]
        public void testB()
        {
            Console.WriteLine("Test B start");
            Thread.Sleep(2000);
            Console.WriteLine("Test B end");
        }
    }
}
