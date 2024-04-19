using Microsoft.AspNetCore.Http;

namespace AngularCrudApI1
{
    public class ErrorHandlerMiddleWare
    {
        /*
         * In ASP.NET Core, RequestDelegate is a delegate type that represents a function that can handle
         * an HTTP request and generate an HTTP response. It is commonly used in middleware components 
         * to define the logic for processing HTTP requests.
         
         public delegate Task RequestDelegate(HttpContext context);

         */

        /*
          It takes an HttpContext object as a parameter, which encapsulates the information about
          the current HTTP request and response. The delegate returns a Task representing 
          the asynchronous processing of the request.

         Middleware components in ASP.NET Core are typically registered using Use extension methods on the
         IApplicationBuilder interface. These extension methods accept a RequestDelegate parameter to
         specify the logic for handling the request.
         */

        /*
         * In the above code, the MyMiddleware class accepts a RequestDelegate parameter in its constructor,
         * which is stored in the _next field. The InvokeAsync method is implemented to perform the 
         * custom middleware logic, and then it calls the next middleware in the pipeline by 
         * invoking the _next delegate.

        By chaining multiple middleware components together using the Use method, each component can
        handle the request and pass it along to the next component in the pipeline until a response is
        generated.
         */

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleWare> _logger;

        public ErrorHandlerMiddleWare(RequestDelegate next, ILogger<ErrorHandlerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        /*
         * The InvokeAsync method is typically used in custom middleware components to perform operations such as authentication,
         * authorization, logging, error handling, and more. It allows you to intercept the request and response at a specific point
         * in the middleware pipeline and execute custom logic.
         */
        public async Task InvokeAsync(HttpContext context)
        {

            // Custom middleware logic here

            try
            {
                //Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // unique id created because based on uniqueid we will log and check the error
                var errorId = Guid.NewGuid();

                // Handle the exception and modify the respoanse
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";

                // Write the error message to the response
                await context.Response.WriteAsync($"An error occurred: {ex.Message}");

                _logger.LogInformation("You can See Global Exception here: ******* ******** ******** ******* ******** ");
                _logger.LogError(ex, $"{errorId} : { ex.Message}");

                #region Here I am just customizing the error for End user to show..Instead of showing all error
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong we are looking into htis resolving this"
                };

                context.Response.WriteAsJsonAsync(error);
                #endregion
            }
        }
    }
}
