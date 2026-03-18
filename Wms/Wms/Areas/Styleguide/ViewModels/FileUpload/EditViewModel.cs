namespace Wms.Areas.Styleguide.ViewModels.FileUpload
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ファイルアップロードStyleGuide
    /// </summary>
    public class EditViewModel
    {
        /// <summary>
        /// 品番ID（ファイルアップロードと同時に投げるFormデータのサンプルとして）
        /// </summary>
        [Display(Name = nameof(Resources.FileUploadResource.ItemId), ResourceType = typeof(Resources.FileUploadResource))]
        public string ItemId { get; set; }
    }
}