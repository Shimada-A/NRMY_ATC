namespace Wms.Common
{
    using System.Web;
    using Share.Extensions.Exceptions;
    using Wms.Areas.Master.Models;

    /// <summary>
    /// ユーザープロファイル
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// ユーザーマスタの情報
        /// </summary>
        public static User User
        {
            get
            {
                if (HttpContext.Current.Items["UserProfile"] == null)
                {
                    return null;
                }

                var user = (User)HttpContext.Current.Items["UserProfile"];
                if (user == null)
                    throw new AuthenticateException();
                return user;
            }
        }
    }
}