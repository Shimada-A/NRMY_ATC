namespace Share.Extensions.Attributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Share.Common.Resources;

    /// <summary>
    /// 小数点のチェック
    /// </summary>
    public class DecimalChkAttribute : ValidationAttribute, IClientValidatable
    {
        public string IntNum { get; private set; }

        private string DecNum { get; set; }

        private string RangeFrom { get; set; }

        private string RangeTo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalChkAttribute"/> class.
        /// </summary>
        /// <param name="intNum"></param>
        /// <param name="decNum"></param>
        /// <param name="rangeFrom"></param>
        /// <param name="rangeTo"></param>
        public DecimalChkAttribute(string intNum, string decNum, string rangeFrom, string rangeTo)
            : base(decNum != "0" ? MessagesResource.ERR_OVER_DECIMAL : MessagesResource.ERR_OVER_INT)
        {
            this.IntNum = intNum;
            this.DecNum = decNum;
            this.RangeFrom = rangeFrom; // -180
            this.RangeTo = rangeTo; // 180
        }

        /// <inheritdoc/>
        public override string FormatErrorMessage(string rangeFrom)
        {
            return string.Format(this.ErrorMessageString, rangeFrom, this.RangeTo, this.IntNum, this.DecNum);
        }

        /// <inheritdoc/>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "decimalchk",

                ErrorMessage = this.FormatErrorMessage(this.RangeFrom),
            };

            rule.ValidationParameters.Add("intnum", this.IntNum);
            rule.ValidationParameters.Add("decnum", this.DecNum);
            rule.ValidationParameters.Add("rangefrom", this.RangeFrom);
            rule.ValidationParameters.Add("rangeto", this.RangeTo);

            // rule.ValidationParameters["intnum"] = IntNum;
            // rule.ValidationParameters["decnum"] = DecNum;
            yield return rule;
        }

        /// <summary>
        /// 小数点の検証する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // if (value != null)
            // {
            //    if (Convert.ToInt64(value) > Convert.ToInt64(RangeTo) || Convert.ToInt64(value) < Convert.ToInt64(RangeFrom))
            //    {
            //        return new ValidationResult(FormatErrorMessage(RangeFrom));
            //    }
            // }
            return ValidationResult.Success;
        }
    }
}