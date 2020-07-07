using System;

namespace SpikeLanguageExt
{
    public class ListExtensionsSamples
    {
        public T Compute<T>(T[] data, T startFrom, Func<T, T, T> action)
            => data
                   .Fold(startFrom, 
                         (acc, item) => action(acc,item));
    }
}
