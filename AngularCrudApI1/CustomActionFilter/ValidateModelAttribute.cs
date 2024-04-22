using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AngularCrudApI1.CustomActionFilter
{
    /// <summary>
    /// ValidateModelAttribute used for validate the model instead of writing Modelsate.isvalid everywhere we can use this filter
    /// </summary>
    public class ValidateModelAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
           if(context.ModelState.IsValid==false)
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
