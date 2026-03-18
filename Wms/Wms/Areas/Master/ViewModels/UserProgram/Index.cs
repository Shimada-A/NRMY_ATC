namespace Wms.Areas.Master.ViewModels.UserProgram
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class UserProgramSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum UserProgramSortKey : byte
        {
            [Display(Name = nameof(Resources.UserProgramResource.ProgramName), ResourceType = typeof(Resources.UserProgramResource))]
            ProgramName,

            [Display(Name = nameof(Resources.UserProgramResource.ProgramClass), ResourceType = typeof(Resources.UserProgramResource))]
            ProgramClass
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
        /// プログラム名
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// プログラム区分
        /// </summary>
        public ProgramClasses ProgramClass { get; set; }

        /// <summary>
        /// 権限レベル
        /// </summary>
        public ProgramPermissionLevelClasses PermissionLevel { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public UserProgramSortKey SortKey { get; set; }

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
    }

    public class UserProgramResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<UserProgramList> UserPrograms { get; set; }
    }

    public class Index
    {
        public UserProgramSearchCondition SearchConditions { get; set; }

        public UserProgramResult UserProgramResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProgramViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new UserProgramSearchCondition();
            this.UserProgramResult = new UserProgramResult();
        }
    }
}