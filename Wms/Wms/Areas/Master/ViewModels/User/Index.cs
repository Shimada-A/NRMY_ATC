namespace Wms.Areas.Master.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class UserSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum UserSortKey : byte
        {
            [Display(Name = nameof(Resources.UserResource.UserId), ResourceType = typeof(Resources.UserResource))]
            UserId,

            [Display(Name = nameof(Resources.UserResource.UserName), ResourceType = typeof(Resources.UserResource))]
            UserName,

            [Display(Name = nameof(Resources.UserResource.CenterIdUserId), ResourceType = typeof(Resources.UserResource))]
            CenterIdUserId,

            [Display(Name = nameof(Resources.UserResource.PermissionLevelUserId), ResourceType = typeof(Resources.UserResource))]
            PermissionLevelUserId
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
        /// List センターコード
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// センター名
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// センター名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public UserSortKey SortKey { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
    }

    public class UserResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<UserList> Users { get; set; }
    }

    public class Index
    {
        public UserSearchCondition SearchConditions { get; set; }

        public UserResult UserResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new UserSearchCondition();
            this.UserResult = new UserResult();
        }
    }
}