namespace AngularCrudApI1.Testing
{  
    public class TransferService
    {
        #region Comments  

        /*We will alter the constructor of the class to accept a parameter , this parameter is going to be a class whichh will 
         encapsulate a validation rule of a  wired transfer, Thus the transfer service class will focous on transfer and the validation
         of this will delegate to another class .... .. and also this allow us to follow the single responsibility */

        #endregion

        private readonly IValidateWireTransfer ValidateWireTransfer;
        public TransferService(IValidateWireTransfer validateWireTransfer)
        {
            this.ValidateWireTransfer = validateWireTransfer;
        }
        
        public void WireTransfer(Account origin, Account destination, decimal amount)
        {
            var state=ValidateWireTransfer.Validate(origin, destination, amount);

            if(!state.IsSuccessful)
            {
                throw new ApplicationException(state.ErrorMessage);
            }
            origin.Funds-=amount;
            destination.Funds += amount;
        }
    }
}
