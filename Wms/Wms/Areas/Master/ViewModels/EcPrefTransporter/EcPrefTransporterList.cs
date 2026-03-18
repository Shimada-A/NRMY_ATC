namespace Wms.Areas.Master.ViewModels.EcPrefTransporter
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 一覧画面明細部
    /// </summary>
    public class EcPrefTransporterList
    {
        /// <summary>
        /// 一覧ラジオボタンチェック状態
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Display(Name = nameof(EcPrefTransporterResource.PrefId), ResourceType = typeof(EcPrefTransporterResource))]
        public string PrefId { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        [Display(Name = nameof(EcPrefTransporterResource.PrefName), ResourceType = typeof(EcPrefTransporterResource))]
        public string PrefName { get; set; }

        /// <summary>
        /// 配送業者ID
        /// </summary>
        [Display(Name = nameof(EcPrefTransporterResource.TransporterId), ResourceType = typeof(EcPrefTransporterResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者名
        /// </summary>
        /// <remarks>
        /// 01:ヤマト運輸
        /// 02:佐川急便
        /// </remarks>
        public string TransporterName { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        public string CenterId { get; set; }

    }
}