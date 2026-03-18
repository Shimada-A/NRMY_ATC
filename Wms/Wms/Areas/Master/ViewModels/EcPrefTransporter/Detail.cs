namespace Wms.Areas.Master.ViewModels.EcPrefTransporter
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 詳細画面
    /// </summary>
    public class Detail
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　（ECセンターのみ必要）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EcPrefTransporterResource.Center), ResourceType = typeof(EcPrefTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫名
        /// </summary>
        /// <remarks>
        /// 倉庫マスタから
        /// </remarks>
        [Display(Name = nameof(EcPrefTransporterResource.Center), ResourceType = typeof(EcPrefTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// 都道府県コード (PREF_ID)
        /// </summary>
        /// <remarks>
        /// 都道府県マスタから
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EcPrefTransporterResource.PrefId), ResourceType = typeof(EcPrefTransporterResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefId { get; set; }

        /// <summary>
        /// 都道府県名 (PREF_NAME)
        /// </summary>
        /// <remarks>
        /// 都道府県マスタから
        /// </remarks>
        [Display(Name = nameof(EcPrefTransporterResource.PrefName), ResourceType = typeof(EcPrefTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefName { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        /// <remarks>
        /// 該当都道府県の配送業者
        /// 01：ヤマト運輸
        /// 02：佐川急便
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EcPrefTransporterResource.TransporterId), ResourceType = typeof(EcPrefTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }

        #endregion プロパティ
    }

}