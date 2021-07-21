using System.Linq;
using LanguageExt;

namespace Validation
{
    public class ValidationBetter
    {
        private const string INVALID_MAIL = "invalid mail";
        private const string INVALID_PASSWORD = "invalid password";
        private const string INVALID_CODE = "invalid code";

        private bool IsEmmailValid(string email)
            => email.Contains("@");

        private bool IsPasswordValid(string password)
            => password.Length > 3;

        private bool IsCodeValid(string code)
            => code.All(char.IsDigit);

        public string Validate(string email, string password, string code)
        {
            if (!IsEmmailValid(email))
                return INVALID_MAIL;
            if (!IsPasswordValid(password))
                return INVALID_PASSWORD;
            if(!IsCodeValid(code))
                return INVALID_CODE;
            return string.Empty;
        }
    }
}