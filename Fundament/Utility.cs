using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Timers;

namespace Integrant.Fundament
{
    public static class Utility
    {
        public static List<string> Qualifications()
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase method     = stackTrace.GetFrame(1).GetMethod();

            string fullName = method.DeclaringType.FullName;
            fullName = fullName.Substring(0, fullName.IndexOf('`'));

            var result = new List<string>();

            var last = "";

            foreach (string s in fullName.Split('.'))
            {
                result.Add(last + s);
                last += s + ".";
            }

            return result;
        }

        public class Debouncer<T>
        {
            public delegate void OnElapsed(T newValue);

            private readonly Timer     _debouncer;
            private readonly OnElapsed _onElapsed;
            private readonly object    _valueLock = new object();

            public Debouncer(OnElapsed onElapsed, T initialValue, int milliseconds = 200)
            {
                _onElapsed = onElapsed;
                Value      = initialValue;
                _debouncer = new Timer(milliseconds);
                _debouncer.Stop();
                _debouncer.AutoReset = false;
                _debouncer.Elapsed += (_, __) =>
                {
                    lock (_valueLock)
                    {
                        _onElapsed.Invoke(Value);
                    }
                };
            }

            public T Value { get; private set; }

            public void Reset(T newValue)
            {
                _debouncer.Stop();
                lock (_valueLock)
                {
                    Value = newValue;
                }

                _debouncer.Start();
            }
        }
    }
}