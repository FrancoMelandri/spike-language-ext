using LanguageExt;

namespace SpikeLanguageExt
{
    public class TryMonad
    {
        public Try<int> DoTry(string id) 
            => () => System.Convert.ToInt32(id);
    }
}