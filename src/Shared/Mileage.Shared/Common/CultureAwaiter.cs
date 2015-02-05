using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Mileage.Shared.Common
{
    public class CultureAwaiter : INotifyCompletion
    { 
        private readonly TaskAwaiter _awaiter; 
        private CultureInfo _culture;
        private CultureInfo _uiCulture;

        public CultureAwaiter(Task task) 
        { 
            if (task == null) 
                throw new ArgumentNullException("task");


            _awaiter = task.GetAwaiter(); 
        }

        public CultureAwaiter GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted 
        {
            get { return _awaiter.IsCompleted; } 
        }

        public void OnCompleted(Action continuation) 
        { 
            _culture = Thread.CurrentThread.CurrentCulture;
            _uiCulture = Thread.CurrentThread.CurrentUICulture;

            _awaiter.OnCompleted(continuation); 
        }

        public void GetResult() 
        { 
            Thread.CurrentThread.CurrentCulture = _culture;
            Thread.CurrentThread.CurrentUICulture = _uiCulture;

            _awaiter.GetResult(); 
        } 
    }

    public class CultureAwaiter<T> : INotifyCompletion
    {
        private readonly TaskAwaiter<T> _awaiter;
        private CultureInfo _culture;
        private CultureInfo _uiCulture;

        public CultureAwaiter(Task<T> task)
        {
            if (task == null)
                throw new ArgumentNullException("task");


            _awaiter = task.GetAwaiter();
        }

        public CultureAwaiter<T> GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted
        {
            get { return _awaiter.IsCompleted; }
        }

        public void OnCompleted(Action continuation)
        {
            _culture = Thread.CurrentThread.CurrentCulture;
            _uiCulture = Thread.CurrentThread.CurrentUICulture;

            _awaiter.OnCompleted(continuation);
        }

        public T GetResult()
        {
            Thread.CurrentThread.CurrentCulture = _culture;
            Thread.CurrentThread.CurrentUICulture = _uiCulture;

            return _awaiter.GetResult();
        }
    }
}