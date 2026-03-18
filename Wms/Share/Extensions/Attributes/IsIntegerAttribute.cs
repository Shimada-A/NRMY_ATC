namespace Share.Extensions.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Share.Common.Resources;

    /// <summary>
    /// 整数検証属性
    /// </summary>
    public class IsIntegerAttribute : ValidationAttribute, IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "isinteger",
                ErrorMessage = MessagesResource.OnlyAllowInteger
            };

            yield return rule;
        }

        /// <summary>
        /// 値が整数のみ使用されているか検証する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return Regex.IsMatch(Convert.ToString(value), "[0-9]+");
        }
    }
}