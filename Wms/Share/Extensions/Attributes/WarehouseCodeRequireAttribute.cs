namespace Share.Extensions.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// WarehouseCode require attribute
    /// Rule: If EcClass = 1 and VirtualFlag = 0 then set require for IfWarehouseCode property
    /// </summary>
    public class WarehouseCodeRequireAttribute : ValidationAttribute, IClientValidatable
    {
        private RequiredAttribute _innerAttribute = new RequiredAttribute();

        private string _depdEcClass;
        private string _depdVirtualFlag;
        private byte _ecClassTargetValue;
        private byte _virtualFlagTargetValue;

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="depdEcClass">EcClass property name</param>
        /// <param name="depdVirtualFlag">VirtualFlag property name</param>
        /// <param name="ecClassTargetValue">EcClass target value</param>
        /// <param name="virtualFlagTargetValue">Virtual Flag target value<</param>
        public WarehouseCodeRequireAttribute(string depdEcClass, string depdVirtualFlag, byte ecClassTargetValue, byte virtualFlagTargetValue)
        {
            this._depdEcClass = depdEcClass;
            this._depdVirtualFlag = depdVirtualFlag;
            this._ecClassTargetValue = ecClassTargetValue;
            this._virtualFlagTargetValue = virtualFlagTargetValue;
        }

        /// <summary>
        /// Override IsValid method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var fieldEcClass = containerType.GetProperty(this._depdEcClass);
            var fieldVirtualFlag = containerType.GetProperty(this._depdVirtualFlag);

            if (fieldEcClass != null && fieldVirtualFlag != null)
            {
                // get the value of the dependent property
                var depdEcClassValue = fieldEcClass.GetValue(validationContext.ObjectInstance, null);
                var depdVirtualFlagValue = fieldVirtualFlag.GetValue(validationContext.ObjectInstance, null);

                if (Convert.ToByte(depdEcClassValue) == this._ecClassTargetValue && Convert.ToByte(depdVirtualFlagValue) == this._virtualFlagTargetValue)
                {
                    // match => means we should try validating this field
                    if (!this._innerAttribute.IsValid(value))

                        // validation failed - return an error
                        return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Implement client validation rule
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "warehousecoderequire",
            };

            string depProp1 = this.BuildDependentPropertyId(metadata, this._depdEcClass);
            string depProp2 = this.BuildDependentPropertyId(metadata, this._depdVirtualFlag);

            rule.ValidationParameters.Add("depdecclass", depProp1);
            rule.ValidationParameters.Add("depdvirtualflag", depProp2);
            rule.ValidationParameters.Add("ecclasstargetvalue", this._ecClassTargetValue);
            rule.ValidationParameters.Add("virtualflagtargetvalue", this._virtualFlagTargetValue);

            yield return rule;
        }

        /// <summary>
        /// Build Dependent property id
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private string BuildDependentPropertyId(ModelMetadata metadata, string property)
        {
            // build the ID of the property
            return metadata.ContainerType.Name + "_" + property;
        }
    }
}