namespace Validation
{
    public partial class ValidationChain
    {
        interface IValidation
        {
            string Validate(string email, string password, string code);
        }
    }
}