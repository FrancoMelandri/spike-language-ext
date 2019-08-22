using LanguageExt;

namespace SpikeLanguageExt
{
    public static class OptionExtension
    {
        public static T OrElse<T>(this Option<T> source, T defaultValue)
        {
            return source.Match<T>(_ => _, () => defaultValue);
        }
    }
}