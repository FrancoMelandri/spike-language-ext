using System.Linq;
using LanguageExt;

namespace Validation
{
    public class ValidationBasic
    {
        private const string INVALID = "invalid";

        private bool IsEmmailValid(string email)
            => email.Contains("@");

        private bool IsPasswordValid(string password)
            => password.Length > 3;

        private bool IsCodeValid(string code)
            => code.All(char.IsDigit);
        
        public string Validate(string email, string password, string code)
        {
            if (IsEmmailValid(email) && 
                IsPasswordValid(password) && 
                IsCodeValid(code))
                return string.Empty;
            return INVALID; 
        }
    }
}