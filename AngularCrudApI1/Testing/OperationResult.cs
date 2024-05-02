namespace AngularCrudApI1.Testing
{
    public class OperationResult
    {
        public OperationResult(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
