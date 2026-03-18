namespace Wms.Areas.Move.ViewModels.InputTransfer
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Move.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintCaseLabelConditions
    {
        /// <summary>
        /// 発行区分
        /// </summary>
        public enum ReleaseClasses : byte
        {
            [Display(Name = nameof(Resources.InputTransferResource.Release), ResourceType = typeof(Resources.InputTransferResource))]
            Release,

            [Display(Name = nameof(Resources.InputTransferResource.AgainRelease), ResourceType = typeof(Resources.InputTransferResource))]
            AgainRelease
        }

        /// <summary>
        /// 発行区分
        /// </summary>
        public ReleaseClasses ReleaseClass { get; set; } = ReleaseClasses.Release;

        /// <summary>
        /// 枚数
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.NumberofSheets), ResourceType = typeof(Resources.InputTransferResource))]
        [Range(1, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte? NumberofSheets { get; set; } = 0;

        /// <summary>
        /// 再発行ケースNo
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.ReleaseBoxNo), ResourceType = typeof(Resources.InputTransferResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        [MinLength(5, ErrorMessageResourceName = nameof(MessageResource.MinLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ReleaseBoxNo { get; set; }
    }
}