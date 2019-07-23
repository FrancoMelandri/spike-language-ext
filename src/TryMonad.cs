using LanguageExt;

namespace SpikeLanguageExt
{
    public class TryMonad
    {
        public Try<int> DoTry(string id) 
        {
            return () => System.Convert.ToInt32(id);
        }
    }
}