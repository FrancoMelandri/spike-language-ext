using System.Linq;

namespace SpikeLanguageExt
{
    public class Validation
    {
        private const string INVALID = "invalid";

        private bool IsEmmailValid(string email)
        {
            return email.Contains("@");
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length > 3;
        }

        private bool IsCodeValid(string code)
        {
            return code.All(char.IsDigit);
        }
        
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