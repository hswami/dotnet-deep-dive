using System;
using System.Linq;

namespace FinalizerDisposable
{
    public static class Program
    {
        public static void Main()
        {
            Test();

            Console.WriteLine();

            Console.Write("Press any key ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static void Test()
        {
            Console.WriteLine("Before using");
            using (var testClass = new TestClass())
            {
                testClass.Data = Enumerable.Range(0, 10).ToArray();
                Console.WriteLine("    Inside using");
            }
            Console.WriteLine("After using");

            Console.WriteLine();

            Console.WriteLine("Before GC.Collect");
            GC.Collect(int.MaxValue, GCCollectionMode.Forced);
            Console.WriteLine("After GC.Collect");
        }
    }

    public class TestClass : IDisposable
    {
        public int[] Data;

        public TestClass()
            => Console.WriteLine("    Constructor called");

        // This won't get called as expected.
        // https://github.com/dotnet/runtime/issues/16028
        ~TestClass()
            => Console.WriteLine("    Finalizer called");

        public void Dispose()
            => Console.WriteLine("    Dispose called");
    }
}
