namespace Validation
{
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
}