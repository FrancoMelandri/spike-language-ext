using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace SpikeLanguageExt
{
    public class ValidationBasic
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

    public partial class ValidationChain
    {
        private const string INVALID_MAIL = "invalid mail";
        private const string INVALID_PASSWORD = "invalid password";
        private const string INVALID_CODE = "invalid code";

        public string Validate(string email, string password, string code)
            => new EmailValidation(
                    new PasswordlValidation(
                        new CodelValidation()))
                .Validate(email, password, code);
    }

    public partial class ValidationChain
    {
        interface IValidation
        {
            string Validate(string email, string password, string code);
        }
    }

    public partial class ValidationChain
    {
        class EmailValidation : IValidation
        {
            IValidation _next;

            public EmailValidation(IValidation next)
            {
                _next = next;
            }

            public string Validate(string email, string password, string code)
            {
                if (email.Contains("@"))
                    return _next.Validate(email, password, code);
                return INVALID_MAIL;
            }
        }
    }
    public partial class ValidationChain
    {
        class PasswordlValidation : IValidation
        {
            IValidation _next;

            public PasswordlValidation(IValidation next)
            {
                _next = next;
            }
            public string Validate(string email, string password, string code)
            {
                if (password.Length > 3)
                    return _next.Validate(email, password, code);
                return INVALID_PASSWORD;
            }
        }
    }
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