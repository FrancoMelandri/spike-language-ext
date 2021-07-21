using LanguageExt;

namespace Extensions
{
    public static class OptionExtension
    {
        public static T OrElse<T>(this Option<T> source, T defaultValue)
            => source.Match<T>(_ => _, () => defaultValue);        
    }
}