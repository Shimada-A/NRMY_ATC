namespace Wms.Areas.Master.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    public class UserLabelReportRowForCsv
    {
        /// <summary>
        /// ユーザーコード
        /// </summary>
        [Display(Name = nameof(UserResource.UserId), ResourceType = typeof(UserResource), Order = 1)]
        public string UserId { get; set; }
    }
}