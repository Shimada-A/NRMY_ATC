namespace Wms.Areas.Styleguide.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Label
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Display(Name = nameof(Resources.LabelResource.SAMPLE_LABEL2), ResourceType = typeof(Resources.LabelResource))]
        public string Name { get; set; }
    }
}