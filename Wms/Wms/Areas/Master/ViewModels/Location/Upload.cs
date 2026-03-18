namespace Wms.Areas.Master.ViewModels.Location
{
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class UploadCondition
    {
        /// <summary>
        /// WorkId
        /// </summary>
        public long WorkId { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;
    }

    public class UploadResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<IndexResultRow> Locations { get; set; }
    }

    public class Upload
    {
        public UploadCondition UploadConditions { get; set; }

        public UploadResult UploadResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationViewModel"/> class.
        /// </summary>
        public Upload()
        {
            this.UploadConditions = new UploadCondition();
            this.UploadResult = new UploadResult();
        }
    }
}