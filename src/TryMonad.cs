using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

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