using Share.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Wms.Areas.Move.ViewModels.InputTransfer
{
    /// <summary>
    /// ShipFrontage which is posted from view
    /// </summary>
    public class SelectedInputTransfer02ViewModel
    {
        /// <summary>
        /// 荷主ID
        /// </summary>
        public string ShipperId { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        public int UpdateCount { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        public string SlipNo { get; set; }

        /// <summary>
        /// 行No
        /// </summary>
        /// <remarks>
        public long SlipSeq { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        public string CenterId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

    }
}