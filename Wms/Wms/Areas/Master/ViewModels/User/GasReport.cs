namespace Wms.Areas.Master.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    public class GasReport
    {
        /// <summary>
        /// 作業者コード
        /// </summary
        [Display(Name = nameof(Resources.UserResource.UserIdDownload), ResourceType = typeof(Resources.UserResource), Order = 1)]
        public string UserId { get; set; }

        /// <summary>
        /// 作業者名称
        /// </summary>
        [Display(Name = nameof(Resources.UserResource.UserNameDownload), ResourceType = typeof(Resources.UserResource), Order = 2)]
        public string UserName { get; set; }

        /// <summary>
        /// 作業者モード
        /// </summary>
        [Display(Name = nameof(Resources.UserResource.UserModeDownload), ResourceType = typeof(Resources.UserResource), Order = 3)]
        public string UserMode { get; set; }
    }
}