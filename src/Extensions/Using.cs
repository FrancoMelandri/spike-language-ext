using System;

namespace Extensions
{
    public static class UsingExtension
    {
        public static void Raw (IDisposable disposable, Action action)
        {
            using (disposable)
            {
                action();
            }
        }

        public static R Using<TDisp, R>(TDisp disposable, Func<TDisp, R> f) where TDisp : IDisposable
        {        
            using (disposable) return f(disposable);     
        }
    }
}