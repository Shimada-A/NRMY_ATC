namespace Wms.Areas.Others.ViewModels.WorkingReference
{
    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class SelectedWorkingReferenceViewModel
    {
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; } = false;

        /// <summary>
        /// ワークID
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }
    }
}