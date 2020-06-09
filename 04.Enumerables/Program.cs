using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Enumerables
{
    public static class Program
    {
        public static void Main()
        {
            var numbersUsingForEachLoopYield = GetNumbersUsingForEachLoopYield();
            Console.WriteLine("FOREACH LOOP YIELD RESULT: {0}", numbersUsingForEachLoopYield.ToArray().ToJson());

            Console.WriteLine();
            Console.WriteLine();

            var numbersUsingForLoopYield = GetNumbersUsingForLoopYield();
            Console.WriteLine("FOR LOOP YIELD     RESULT: {0}", numbersUsingForLoopYield.ToArray().ToJson());

            Console.WriteLine();
            Console.WriteLine();

            var numbersUsingYieldOnly = GetNumbersUsingYieldOnly();
            Console.WriteLine("YIELD ONLY         RESULT: {0}", numbersUsingYieldOnly.ToArray().ToJson());

            Console.WriteLine();
            Console.WriteLine();

            var numbersUsingCustomIEnumerable = GetNumbersUsingCustomIEnumerable();
            Console.WriteLine("CUSTOM IENUMERABLE RESULT: {0}", numbersUsingCustomIEnumerable.ToArray().ToJson());

            Console.WriteLine();
            Console.WriteLine();

            Console.Write("Press any key ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static IEnumerable<int?> GetNumbersUsingForEachLoopYield()
        {
            foreach (var number in Enumerable.Range(0, 10))
                yield return number;
        }

        private static IEnumerable<int?> GetNumbersUsingForLoopYield()
        {
            for (var number = 0; number != 10; ++number)
                yield return number;
        }

        private static IEnumerable<int?> GetNumbersUsingYieldOnly()
        {
            yield return 0;
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
            yield return 6;
            yield return 7;
            yield return 8;
            yield return 9;
        }

        private static IEnumerable<int?> GetNumbersUsingCustomIEnumerable()
            => new Numbers();

        private static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);
    }

    public class Numbers : IEnumerable<int?>
    {
        private readonly int?[] _numbers =
            Enumerable.Range(0, 10).Cast<int?>().ToArray();

        public IEnumerator<int?> GetEnumerator()
            => new ArrayEnumerator<int?>(_numbers);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    public class ArrayEnumerator<T> : IEnumerator<T>
    {
        private int _index = -1;
        private readonly T[] _array;

        public ArrayEnumerator(T[] array)
            => _array = array;

        public bool MoveNext()
            => ++_index < _array.Length;

        public void Reset()
            => _index = -1;

        public T Current
            => _index < 0 ? default : _array[_index];

        object IEnumerator.Current
            => Current;

        /// <inheritdoc />
        public void Dispose() { }
    }
}
