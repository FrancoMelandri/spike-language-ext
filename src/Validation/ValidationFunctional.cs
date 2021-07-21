using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Validation
{
    public class ValidationFunctional
    {
        private const string INVALID_MAIL = "invalid mail";
        private const string INVALID_PASSWORD = "invalid password";
        private const string INVALID_CODE = "invalid code";

        Either<string, Unit> ValidateMail(string email) 
            => email.Contains("@") ?
                Right<string, Unit>(Unit.Default) :
                INVALID_MAIL;

        Either<string, Unit> ValidatePassword(string password) 
            => password.Length > 3 ?
                Right<string, Unit>(Unit.Default) :
                INVALID_PASSWORD;

        Either<string, Unit> ValidateCode(string code) 
            => code.All(char.IsDigit) ?
                Right<string, Unit>(Unit.Default) :
                INVALID_CODE;

        public string Validate(string email, string password, string code)
            => ValidateMail(email)
                .Bind(_ => ValidatePassword(password))
                .Bind(_ => ValidateCode(code))
                .IfRight(_ => string.Empty);

        public string Validate1(string email, string password, string code)
            => (from x in ValidateMail(email)
                    from y in ValidatePassword(password)
                    from z in ValidateCode(code)
                    select x + y + z)
                .IfRight(_ => string.Empty);
    }
}