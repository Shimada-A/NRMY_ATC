namespace Wms.Areas.Master.ViewModels.BoxSetting
{
    /// <summary>
    /// BoxSetting which is posted from view
    /// </summary>
    public class SelectedBoxSettingViewModel
    {
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        public string ShipperId { get; set; }

        /// <summary>
        /// 設定ID (BOX_SETTINGS_ID)
        /// </summary>
        /// <remarks>
        /// 連番　(重複不可項目については備考参照)
        /// </remarks>
        public int BoxSettingsId { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }
    }
}