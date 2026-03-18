namespace Share.Extensions.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Share.Common.Resources;

    public class MinValueAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly int _minValue;

        public MinValueAttribute(int minValue)
        {
            this._minValue = minValue;
            this.ErrorMessage = string.Format(MessagesResource.MinValueMessage, this._minValue);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return Convert.ToInt32(value) >= this._minValue;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = string.Format(MessagesResource.MinValueMessage, this._minValue)
            };
            rule.ValidationParameters.Add("min", this._minValue);
            rule.ValidationParameters.Add("max", int.MaxValue);
            rule.ValidationType = "range";
            yield return rule;
        }
    }
}