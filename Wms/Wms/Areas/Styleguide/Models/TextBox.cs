namespace Wms.Areas.Styleguide.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TextBox
    {
        public string SingleLine1 { get; set; }

        [DataType(DataType.MultilineText)]
        public string MultiLine1 { get; set; }

        public string SingleLine2 { get; set; }

        // TextAreaForを使うときはDataType属性は不要
        public string MultiLine2 { get; set; }

        [Display(Name = nameof(Resources.TextBoxResource.LayoutText), ResourceType = typeof(Resources.TextBoxResource))]
        public string LayoutText { get; set; }

        [MaxLength(10, ErrorMessageResourceName = nameof(Share.Common.Resources.MessagesResource.MaxLength), ErrorMessageResourceType = typeof(Share.Common.Resources.MessagesResource))]
        [Required(ErrorMessageResourceName = nameof(Share.Common.Resources.MessagesResource.Required), ErrorMessageResourceType = typeof(Share.Common.Resources.MessagesResource))]
        [Display(Name = nameof(Resources.TextBoxResource.ValidationText), ResourceType = typeof(Resources.TextBoxResource))]
        public string ValidationText { get; set; }

        [Required(ErrorMessageResourceName = nameof(Share.Common.Resources.MessagesResource.Required), ErrorMessageResourceType = typeof(Share.Common.Resources.MessagesResource))]
        [Display(Name = nameof(Resources.TextBoxResource.NumberField), ResourceType = typeof(Resources.TextBoxResource))]
        public int NumberField { get; set; }
    }
}