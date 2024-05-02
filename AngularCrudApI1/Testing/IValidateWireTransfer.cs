namespace AngularCrudApI1.Testing
{
    public interface IValidateWireTransfer
    {
        OperationResult Validate(Account origin, Account destination, decimal amount);
    }
}
