using System;
using System.Collections.Generic;
using System.Linq;

namespace Mileage.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Traverses the whole object specified by the <paramref name="childSelector"/>.
        /// </summary>
        /// <typeparam name="T">The type of items to traverse.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="childSelector">The child selector.</param>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();

                yield return next;

                foreach(var child in childSelector(next))
                    stack.Push(child);
            }
        }
    }
}