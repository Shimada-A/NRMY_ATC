namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class TransferReference01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum TransferReference01SortKey : byte
        {
            [Display(Name = nameof(Resources.TransferReferenceResource.TransferClassDenpyoDateStoreId), ResourceType = typeof(Resources.TransferReferenceResource))]
            TransferClassDenpyoDateStoreId,

            [Display(Name = nameof(Resources.TransferReferenceResource.DenpyoDateTransferClassStoreId), ResourceType = typeof(Resources.TransferReferenceResource))]
            DenpyoDateTransferClassStoreId,

            [Display(Name = nameof(Resources.TransferReferenceResource.StoreIdDenpyoDate), ResourceType = typeof(Resources.TransferReferenceResource))]
            StoreIdDenpyoDate
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// 入荷日区分
        /// </summary>
        public enum ArriveDateClasses
        {
            [Display(Name = nameof(Resources.TransferReferenceResource.ArrivePlanDate), ResourceType = typeof(Resources.TransferReferenceResource))]
            ArrivePlanDate = 1,

            [Display(Name = nameof(Resources.TransferReferenceResource.TransferResultDate), ResourceType = typeof(Resources.TransferReferenceResource))]
            TransferResultDate = 2
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 移動区分(2:店舗返品)
        /// </summary>
        public bool StoreReturnFlag { get; set; } = true;

        /// <summary>
        /// 移動区分(1:拠点間移動)
        /// </summary>
        public bool BaseMoveFlag { get; set; } = true;

        /// <summary>
        /// 移動区分(3:拠点間移動(WMSなし倉庫))
        /// </summary>
        public bool BaseMoveNoWmsCenterFlag { get; set; } = true;

        /// <summary>
        /// 入荷日区分(入荷予定日または入荷実績日)
        /// </summary>
        public ArriveDateClasses ArriveDateClass { get; set; } = ArriveDateClasses.ArrivePlanDate;

        /// <summary>
        /// 入荷予定日From
        /// </summary>
        public DateTime? ArrivePlanDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷予定日To
        /// </summary>
        public DateTime? ArrivePlanDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷実績日From
        /// </summary>
        public DateTime? TransferResultDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷実績日To
        /// </summary>
        public DateTime? TransferResultDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 過去分を含む
        /// </summary>
        public bool ContainsArchive { get; set; }

        /// <summary>
        /// 移動元センター
        /// </summary>
        public string TransferFromCenterId { get; set; }

        /// <summary>
        /// 移動元店舗
        /// </summary>
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元店舗
        /// </summary>
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        public string TransferStatus { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string SlipNo { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        public int UnplannedFlag { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public TransferReference01SortKey SortKey { get; set; } = TransferReference01SortKey.TransferClassDenpyoDateStoreId;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQtySum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQtySum { get; set; }

        /// <summary>
        /// 実績数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoPlanQtySum { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoResultQtySum { get; set; }

        /// <summary>
        /// アイテムコード
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 伝票日付From
        /// </summary>
        public DateTime? DenpyoDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 伝票日付To
        /// </summary>
        public DateTime? DenpyoDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 伝票番号
        /// </summary>
        public string DenpyoNo { get; set; }
    }
}