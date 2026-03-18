namespace Wms.Areas.Styleguide.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RangeSlider
    {
        [Display(Name = nameof(Resources.RangeSliderResource.SellingPriceStart), ResourceType = typeof(Resources.RangeSliderResource))]
        public int? SellingPriceStart { get; set; }

        [Display(Name = nameof(Resources.RangeSliderResource.SellingPriceEnd), ResourceType = typeof(Resources.RangeSliderResource))]
        public int? SellingPriceEnd { get; set; }
    }
}