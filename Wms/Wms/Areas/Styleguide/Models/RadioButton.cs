namespace Wms.Areas.Styleguide.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class RadioButton
    {
        public List<SelectListItem> SampleRadio1 { get; set; }

        public SampleEnum SampleRadio2 { get; set; }

        public enum SampleEnum : byte
        {
            Orange = 1,
            Apple = 2,
        }
    }
}