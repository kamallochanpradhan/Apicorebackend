namespace AngularCrudApI1.Testing
{
    public class WireTransferValidator : IValidateWireTransfer
    {
        public OperationResult Validate(Account origin, Account destination, decimal amount)
        {
            if(amount > origin.Funds)
            {
                return new OperationResult(false, "The origin account does not have enough funds available");
            }

            //Can do other validation

            return new OperationResult(true,"Success");

        }
    }
}
