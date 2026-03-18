namespace Share.Extensions.Classes
{
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Extensions.Attributes;

    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string)
                && !propertyDescriptor.Attributes.Cast<object>().Any(a => a.GetType() == typeof(NoTrimAttribute)))
            {
                if (value != null)
                {
                    var strValue = value.ToString();
                    if (!string.IsNullOrWhiteSpace(strValue))
                    {
                        value = strValue.Trim();
                    }
                }
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}