namespace Wms.Areas.Returns.ViewModels.EcReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using System.Collections.Generic;


    public class EcReference01SearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// 返品登録日From
        /// </summary>
        public DateTime? ArriveDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 返品登録日To
        /// </summary>
        public DateTime? ArriveDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// EC注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        public string ReturnId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum DataSortKey : byte
        {
            [Display(Name = nameof(Resources.EcReferenceResource.ArriveShpIns), ResourceType = typeof(Resources.EcReferenceResource))]
            ArriveShpIns,

            [Display(Name = nameof(Resources.EcReferenceResource.ShpInsArrive), ResourceType = typeof(Resources.EcReferenceResource))]
            ShpInsArrive
        }

        /// <summary>
        /// Sort key
        /// </summary>
        public DataSortKey SortKey { get; set; } = DataSortKey.ArriveShpIns;

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
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Result Type
        /// </summary>
        public ResultTypes ResultType { get; set; } = ResultTypes.Stock;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        public IList<SelectedEcReference01ViewModel> EcReference01s { get; set; }
    }


}