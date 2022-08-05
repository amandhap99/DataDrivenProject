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
    public class TestA
    {
        [Test]
        public void testA()
        {
            Console.WriteLine("Test A start");
            Thread.Sleep(2000);
            Console.WriteLine("Test A end");
        }
    }
}
