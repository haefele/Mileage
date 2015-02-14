﻿using System.Threading.Tasks;
using Mileage.Shared.Common;

namespace Mileage.Shared.Extensions
{
    public static class TaskExtensions
    {
        public static CultureAwaiter WithCurrentCulture(this Task task)
        {
            return new CultureAwaiter(task);
        }

        public static CultureAwaiter<T> WithCurrentCulture<T>(this Task<T> task)
        {
            return new CultureAwaiter<T>(task);
        }
    }
}