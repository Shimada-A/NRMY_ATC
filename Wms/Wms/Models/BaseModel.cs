namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Share.Common.Resources;
    using Wms.Common;

    /// <summary>
    /// モデルの基底クラス
    /// </summary>
    public partial class BaseModel
    {
        //public System.Data.Common.DbConnection DbContext = MvcDbContext.Current.Database.Connection;

        #region プロパティ

        /// <summary>
        /// 作成日付
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Column(Order = 1)]
        public DateTimeOffset MakeDate { get; set; }

        /// <summary>
        /// 作成ユーザID
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Column(Order = 2)]
        public string MakeUserId { get; set; } = "0001";

        /// <summary>
        /// 作成プログラム名
        /// </summary>
        [MaxLength(61, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Column(Order = 3)]
        public string MakeProgramName { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Column(Order = 4)]
        public DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// 更新ユーザID
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Column(Order = 5)]
        public string UpdateUserId { get; set; } = "WG";

        /// <summary>
        /// 更新プログラム名
        /// </summary>
        [MaxLength(61, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Column(Order = 6)]
        public string UpdateProgramName { get; set; }

        /// <summary>
        /// 更新回数
        /// </summary>
        [ConcurrencyCheck]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Column(Order = 7)]
        public int UpdateCount { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        [Key]
        [Column(Order = 99)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipperId { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// 属性で操作できないDBの設定をする
        /// </summary>
        /// <typeparam name="T">BaseModelの派生クラスを条件にする</typeparam>
        /// <param name="modelBuilder">DBコンテキストのモデルを定義するビルダー（MvcDbContext.OnModelCreating参照）</param>
        /// <param name="subClass">派生クラス（ジェネリック型を推論させるために必要な引数）</param>
        public virtual void OnModelCreating<T>(DbModelBuilder modelBuilder, T subClass) where T : BaseModel
        {
        }

        /// <summary>
        /// 監査情報をセット
        /// </summary>
        public void SetBaseInfo()
        {
            // TODO:参照がなくなったら削除する。
            throw new Exception("SetBaseInfoInsertまたは、SetBaseInfoUpdateを使ってください。Please use SetBaseInfoInsert or SetBaseInfoUpdate.");

            // MakeDate = (MakeDate == DateTimeOffset.MinValue) ? DateTimeOffset.Now : MakeDate;
            // UpdateDate = DateTimeOffset.Now;
        }

        /// <summary>
        /// Set Base info of Data Insert
        /// </summary>
        public void SetBaseInfoInsert()
        {
            this.MakeDate = (this.MakeDate == DateTimeOffset.MinValue) ? DateTimeOffset.Now : this.MakeDate;
            this.MakeUserId = Profile.User.UserId;
            this.ShipperId = Profile.User.ShipperId;
            this.MakeProgramName = GetProgramId();

            this.UpdateDate = this.MakeDate;
            this.UpdateUserId = this.MakeUserId;
            this.UpdateProgramName = this.MakeProgramName;
        }

        /// <summary>
        /// Set Base info of Data Update
        /// </summary>
        public void SetBaseInfoUpdate()
        {
            this.UpdateDate = DateTimeOffset.Now;
            this.UpdateUserId = Profile.User.UserId;
            this.UpdateProgramName = GetProgramId();
            this.UpdateCount += 1;
        }

        /// <summary>
        /// Get Program Id
        /// </summary>
        /// <returns></returns>
        public static string GetProgramId()
        {
            // Get roote data
            var rooteData = System.Web.HttpContext.Current.Request.RequestContext.RouteData;
            if (rooteData != null)
            {
                // Get controller name, action name
                string controllerName = rooteData.Values != null && rooteData.Values.ContainsKey("controller") ? rooteData.Values["controller"].ToString() : string.Empty;
                string actionName = rooteData.Values != null && rooteData.Values.ContainsKey("action") ? rooteData.Values["action"].ToString() : string.Empty;

                // In the case of controller name is not empty
                if (!string.IsNullOrEmpty(controllerName))
                {
                    // Get area name
                    string areaName = rooteData.DataTokens != null && rooteData.DataTokens.ContainsKey("area") ? rooteData.DataTokens["area"].ToString() : string.Empty;

                    // Return program id
                    if (areaName == string.Empty)
                    {
                        return controllerName + "/" + actionName;
                    }
                    else
                    {
                        return areaName + "/" + controllerName + "/" + actionName;
                    }
                }
            }

            return string.Empty;
        }

        #endregion
    }
}