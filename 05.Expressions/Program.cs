using System;
using System.Linq;
using System.Linq.Expressions;

using Newtonsoft.Json;

namespace Expressions
{
    public static class Program
    {
        public static void Main()
        {
            FactorialExample();

            Console.WriteLine();
            Console.WriteLine();

            LinqExample();

            Console.WriteLine();
            Console.WriteLine();

            Console.Write("Press any key ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static void FactorialExample()
        {
            Func<int, int> factorialDelegate = Factorial;
            Console.WriteLine("DELEGATE                 : {0}", factorialDelegate);
            Console.WriteLine("RESULT                   : {0}", factorialDelegate(5));

            Console.WriteLine();

            Expression<Func<int, int>> factorialInlineExpression = n => Factorial(n);
            Console.WriteLine("INLINE EXPRESSION        : {0}", factorialInlineExpression);
            Console.WriteLine("INLINE EXPRESSION BODY   : {0}", factorialInlineExpression.Body);
            Console.WriteLine("RESULT                   : {0}", factorialInlineExpression.Compile()(5));

            Console.WriteLine();

            var parameterExpression = Expression.Parameter(typeof (int), "n");
            var expressionTree = Expression.Call(
                typeof (Program),
                nameof (Factorial),
                new Type[0],
                parameterExpression);
            Console.WriteLine("EXPRESSION TREE          : {0}", expressionTree);
            Console.WriteLine("RESULT (KNOWN   DELEGATE): {0}", Expression.Lambda<Func<int, int>>(expressionTree, parameterExpression).Compile()(5));
            Console.WriteLine("RESULT (UNKNOWN DELEGATE): {0}", Expression.Lambda(expressionTree, parameterExpression).Compile().DynamicInvoke(5));
        }

        private static int Factorial(int n)
            => n > 1 ? n * Factorial(n - 1) : 1;

        private static void LinqExample()
        {
            const int count = 10;
            var random = new Random();
            var array = Enumerable.Range(0, count).ToArray();
            for (var index1 = 0; index1 < count - 1; ++index1)
            {
                var index2 = random.Next(index1 + 1, count - 1);
                var temporary = array[index2];
                array[index2] = array[index1];
                array[index1] = temporary;
            }

            LinqExampleWithKeywords(array);

            Console.WriteLine();

            LinqExampleWithFluentAPI(array);

            Console.WriteLine();

            LinqExampleWithInlineExpression(array);

            Console.WriteLine();

            LinqExampleWithExpressionTree(array);
        }

        private static void LinqExampleWithKeywords(int[] array)
        {
            var keywordQuery =
                from item in array
                orderby item
                group item by item < 5 into items
                select new Result(
                    items.Key,
                    items.ToArray());
            Console.WriteLine("KEYWORD          : {0}", keywordQuery);
            Console.WriteLine("RESULTS          : {0}", keywordQuery.ToArray().ToJson());
        }

        private static void LinqExampleWithFluentAPI(int[] array)
        {
            var fluentAPIQuery = array
                .OrderBy(item => item)
                .GroupBy(item => item < 5)
                .Select(
                    items => new Result(
                        items.Key,
                        items.ToArray()));
            Console.WriteLine("FLUENT API       : {0}", fluentAPIQuery);
            Console.WriteLine("RESULTS          : {0}", fluentAPIQuery.ToArray().ToJson());
        }

        private static void LinqExampleWithInlineExpression(int[] array)
        {
            Expression<Func<int[], Result[]>> inlineExpression =
                data => data
                    .OrderBy(item => item)
                    .GroupBy(item => item < 5)
                    .Select(
                        items => new Result(
                            items.Key,
                            items.ToArray()))
                    .ToArray();
            var inlineExpressionResults = inlineExpression.Compile().DynamicInvoke(array);
            Console.WriteLine("INLINE EXPRESSION: {0}", inlineExpression);
            Console.WriteLine("RESULTS          : {0}", inlineExpressionResults.ToJson());
        }

        private static void LinqExampleWithExpressionTree(int[] array)
        {
            var itemsParameter = Expression.Parameter(typeof (IGrouping<bool, int>), "items");
            var dataParameter = Expression.Parameter(typeof (int[]), "data");
            var itemParameter = Expression.Parameter(typeof (int), "item");
            var orderByExpression =
                Expression.Call(
                    typeof (Enumerable),
                    nameof (Enumerable.OrderBy),
                    new[]
                    {
                        typeof (int),
                        typeof (int)
                    },
                    dataParameter,
                    Expression.Lambda(
                        itemParameter,
                        itemParameter));
            var groupByExpression =
                Expression.Call(
                    typeof (Enumerable),
                    nameof (Enumerable.GroupBy),
                    new[]
                    {
                        typeof (int),
                        typeof (bool)
                    },
                    orderByExpression,
                    Expression.Lambda(
                        Expression.LessThan(
                            itemParameter,
                            Expression.Constant(5)),
                        itemParameter));
            var selectExpression =
                Expression.Call(
                    typeof (Enumerable),
                    nameof (Enumerable.Select),
                    new[]
                    {
                        typeof (IGrouping<bool, int>),
                        typeof (Result)
                    },
                    groupByExpression,
                    Expression.Lambda(
                        Expression.New(
                            typeof (Result)
                                .GetConstructor(
                                    new[]
                                    {
                                        typeof (bool),
                                        typeof (int[])
                                    }),
                            Expression.Property(
                                itemsParameter,
                                nameof (IGrouping<bool, int>.Key)),
                            Expression.Call(
                                typeof (Enumerable),
                                nameof (Enumerable.ToArray),
                                new[] { typeof (int) },
                                itemsParameter)),
                        itemsParameter));
            var toArrayExpression =
                Expression.Call(
                    typeof (Enumerable),
                    nameof (Enumerable.ToArray),
                    new[] { typeof (Result) },
                    selectExpression);
            var expressionTree =
                Expression.Lambda(
                    toArrayExpression,
                    dataParameter);
            Console.WriteLine("EXPRESSION TREE  : {0}", expressionTree);
            Console.WriteLine("RESULTS          : {0}", expressionTree.Compile().DynamicInvoke(array).ToJson());
        }

        private static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);
    }

    public class Result
    {
        public bool LessThan5;
        public int[] Items;

        public Result(
            bool lessThan5,
            int[] items)
        {
            LessThan5 = lessThan5;
            Items = items;
        }
    }
}
