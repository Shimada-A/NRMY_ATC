namespace Wms.Areas.Master.ViewModels.DeliareaGroupSearchModal
{
    using System.ComponentModel.DataAnnotations;

    public partial class DeliareaGroupViewModel
    {
        public bool IsCheck { get; set; }

        [Display(Name = nameof(Resources.DeliareaGroupResource.CenterId), ResourceType = typeof(Resources.DeliareaGroupResource))]
        public string CenterId { get; set; }

        [Display(Name = nameof(Resources.DeliareaGroupResource.DeliareaGroupId), ResourceType = typeof(Resources.DeliareaGroupResource))]
        public string DeliareaGroupId { get; set; }

        [Display(Name = nameof(Resources.DeliareaGroupResource.DeliareaGroupId), ResourceType = typeof(Resources.DeliareaGroupResource))]
        public string DeliareaGroupName { get; set; }

        [Display(Name = nameof(Resources.DeliareaGroupResource.PrefId), ResourceType = typeof(Resources.DeliareaGroupResource))]
        public string PrefId { get; set; }

        [Display(Name = nameof(Resources.DeliareaGroupResource.PrefName), ResourceType = typeof(Resources.DeliareaGroupResource))]
        public string PrefName { get; set; }
        public string GenCd { get; set; }
    }
}