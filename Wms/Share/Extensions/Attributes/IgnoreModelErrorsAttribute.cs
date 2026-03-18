namespace Share.Extensions.Attributes
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Ignore validate for  filed of modle
    /// </summary>
    public class IgnoreModelErrorsAttribute : ActionFilterAttribute
    {
        private string keysString;

        public IgnoreModelErrorsAttribute(string keys)
            : base()
        {
            this.keysString = keys;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.ViewData.ModelState;
            string[] keyPatterns = this.keysString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in keyPatterns)
            {
                if (modelState[key] != null)
                {
                    modelState[key].Errors.Clear();
                }
            }
        }
    }
}
