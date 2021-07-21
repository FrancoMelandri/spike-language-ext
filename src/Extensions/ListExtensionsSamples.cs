using System;

namespace Extensions
{
    public class ListExtensionsSamples
    {
        public T Compute<T>(T[] data, T startFrom, Func<T, T, T> action)
            => data
                .Fold(startFrom, 
                      (acc, item) => action(acc,item));

        public S PartialSum<T, S>(T[] data, S startFrom, Func<S, T, S> action, Func<T, bool> condition)
            => data
                .FoldWhile(startFrom,
                           (acc, item) => action(acc, item),
                           acc => condition(acc));
    }
}
