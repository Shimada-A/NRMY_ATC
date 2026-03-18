namespace Share.Extensions.Attributes
{
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Validate for only include value
    /// </summary>
    public class ValidateIncludeValue : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            var valueProvider = filterContext.Controller.ValueProvider;

            var keysWithNoIncomingValue = modelState.Keys.Where(x => !valueProvider.ContainsPrefix(x));
            if (keysWithNoIncomingValue != null)
            {
                foreach (var key in keysWithNoIncomingValue)
                {
                    modelState[key].Errors.Clear();
                }
            }
        }
    }
}