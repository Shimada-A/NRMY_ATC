namespace Share.Extensions.Exceptions
{
    using System;
    using System.Web;
    using System.Web.Security;

    /// <summary>
    /// 認証の例外
    /// </summary>
    /// <remarks>
    /// ログインが失敗したときには使わない。(ID、パスワードの不一致など)
    /// </remarks>
    [Serializable]
    public class AuthenticateException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AuthenticateException() : base("認証情報が無効です。ログインからやり直してください。")
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
        }

        public AuthenticateException(string message) : base(message)
        {
        }

        public AuthenticateException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AuthenticateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}