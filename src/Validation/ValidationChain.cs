namespace Validation
{
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
}