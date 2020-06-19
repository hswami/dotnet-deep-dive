using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Newtonsoft.Json;

// Example Implementation #1
// https://docs.microsoft.com/en-us/archive/blogs/mattwar/linq-building-an-iqueryable-provider-part-i
// https://docs.microsoft.com/en-us/archive/blogs/mattwar/linq-building-an-iqueryable-provider-part-ii

// Example Implementation #2
// https://putridparrot.com/blog/creating-a-custom-linq-provider/

// Production Implementation
// https://github.com/re-motion/Relinq

namespace Queryables
{
    public static class Program
    {
        public static void Main()
        {
            var numbersQuery = new NumbersQueryable().Where(number => number.Value < 5);
            Console.WriteLine("QUERY  : {0}", numbersQuery.Expression);

            var numbersResults = numbersQuery.ToArray();
            Console.WriteLine("RESULTS: {0}", numbersResults.ToJson());

            Console.WriteLine();

            Console.Write("Press any key ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);
    }

    public class NumbersQueryable : IOrderedQueryable<Number>
    {
        public NumbersQueryable()
        {
            Expression = Expression.Constant(this);
            Provider = new NumbersQueryProvider(this);
        }

        public IEnumerator<Number> GetEnumerator()
            => Provider.Execute<IEnumerator<Number>>(Expression);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public Type ElementType
            => typeof (Number);

        public Expression Expression { get; set; }

        public IQueryProvider Provider { get; }
    }

    public class NumbersQueryProvider : IQueryProvider
    {
        private readonly Number[] _array =
            Enumerable
                .Range(0, 10)
                .Select(n => new Number(n))
                .ToArray();

        private readonly NumbersQueryable _queryable;

        public NumbersQueryProvider(NumbersQueryable queryable)
            => _queryable = queryable;

        public IQueryable CreateQuery(Expression expression)
            => CreateQuery<Number>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof (TElement) != typeof (Number))
                throw new Exception("Only " + typeof (Number).FullName + " objects are supported.");

            _queryable.Expression = expression;

            return (IQueryable<TElement>) _queryable;
        }

        object IQueryProvider.Execute(Expression expression)
            => Execute<IEnumerator<Number>>(expression);

        public TResult Execute<TResult>(Expression expression)
        {
            if (typeof (TResult) != typeof (IEnumerator<Number>))
                throw new Exception("Only " + typeof (Number).FullName + " objects are supported.");

            var expressionVisitor = new NumbersQueryExpressionVisitor(_array);
            expressionVisitor.Visit(_queryable.Expression);

            return (TResult) expressionVisitor.Results.GetEnumerator();
        }
    }

    public class NumbersQueryExpressionVisitor : ExpressionVisitor
    {
        public IList<Number> Results;

        public NumbersQueryExpressionVisitor(Number[] results)
            => Results = results.ToList();

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left is MemberExpression member &&
                member.Member.Name == nameof (Number.Value) &&
                node.Right is ConstantExpression constant)
                Results = Results.Where(n => n.Value < (int) constant.Value).ToArray();
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof (Queryable) &&
                node.Method.Name == nameof (Queryable.Where))
                for (var index = 0; index != node.Arguments.Count; ++index)
                    Visit(node.Arguments[index]);
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            Visit(node.Operand);
            return node;
        }
    }

    public class Number
    {
        public int Value;

        public Number(int value)
            => Value = value;
    }
}
