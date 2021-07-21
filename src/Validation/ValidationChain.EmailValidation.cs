namespace Validation
{
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
}