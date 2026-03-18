using System;

namespace Wms.Areas.Stock.ViewModels.LocMove
{
    /// <summary>
    /// ShipFrontage which is posted from view
    /// </summary>
    [Serializable]
    public class SelectedLocMoveViewModel
    {
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; } = false;

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

        /// <summary>
        /// ロケーションコード
        /// </summary>
        public string LocationCd { get; set; }

    }
}