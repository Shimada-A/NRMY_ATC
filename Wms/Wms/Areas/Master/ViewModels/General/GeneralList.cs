namespace Wms.Areas.Master.ViewModels.General
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class GeneralList
    {
        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public string UpdateCount { get; set; }

        /// <summary>
        /// 荷主ID (SHIPPER_ID)
        /// </summary>
        public string ShipperId { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(GeneralResource.CenterId), ResourceType = typeof(GeneralResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫名 (CENTER_NAME)
        /// </summary>
        [Display(Name = nameof(GeneralResource.CenterId), ResourceType = typeof(GeneralResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// 登録分類コード (REGISTER_DIVI_CD)
        /// </summary>
        [Display(Name = nameof(GeneralResource.RegisterDiviCd), ResourceType = typeof(GeneralResource))]
        public string RegisterDiviCd { get; set; }

        /// <summary>
        /// 汎用分類コード (GEN_DIV_CD)
        /// </summary>
        [Display(Name = nameof(GeneralResource.GenDivCd), ResourceType = typeof(GeneralResource))]
        public string GenDivCd { get; set; }

        /// <summary>
        /// 汎用分類名 (GEN_DIV_CD)
        /// </summary>
        [Display(Name = nameof(GeneralResource.GenDivCd), ResourceType = typeof(GeneralResource))]
        public string GenDivName { get; set; }

        /// <summary>
        /// 汎用コード (GEN_CD)
        /// </summary>
        [Display(Name = nameof(GeneralResource.GenCd), ResourceType = typeof(GeneralResource))]
        public string GenCd { get; set; }

        /// <summary>
        /// 汎用値 (GEN_NAME)
        /// </summary>
        [Display(Name = nameof(GeneralResource.GenName), ResourceType = typeof(GeneralResource))]
        public string GenName { get; set; }

        /// <summary>
        /// 並び順 (ORDER_NO)
        /// </summary>
        [Display(Name = nameof(GeneralResource.OrderNo), ResourceType = typeof(GeneralResource))]
        public string OrderNo { get; set; }
        public string Rid { get; set; }
    }
}