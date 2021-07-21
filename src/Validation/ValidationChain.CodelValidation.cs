using System.Linq;
using LanguageExt;

namespace Validation
{
    public partial class ValidationChain
    {
        class CodelValidation : IValidation
        {
            public string Validate(string email, string password, string code)
            {
                if (code.All(char.IsDigit))
                    return string.Empty;
                return INVALID_CODE;
            }
        }
    }
}