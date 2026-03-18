namespace Share.Extensions.Classes
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Web;
    using Newtonsoft.Json;
    using Share.Common.Resources;

    /// <summary>
    /// Cookieに関する拡張
    /// </summary>
    public static class CookieExtention
    {
        /// <summary>
        /// Cookieの値を指定した型で取り出します。
        /// </summary>
        /// <typeparam name="T">戻り値の型</typeparam>
        /// <param name="httpCookie">拡張対象のクラス</param>
        /// <param name="name">Cookieのキー</param>
        /// <returns></returns>
        public static T Get<T>(this HttpCookieCollection httpCookie, string name)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(name)?.Value;
            if (string.IsNullOrWhiteSpace(cookie)) return default;

            using (var ms = new MemoryStream(Convert.FromBase64String(cookie.Replace('-', '+').Replace('_', '/'))))
            using (var mso = new MemoryStream())
            {
                using (var zs = new DeflateStream(ms, CompressionMode.Decompress)) zs.CopyTo(mso);

                var json = Encoding.UTF8.GetString(mso.ToArray());
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        /// <summary>
        /// 検索条件をcookieに保存する
        /// </summary>
        /// <param name="name">Cookieのキー</param>
        /// <param name="value">検索条件</param>
        public static void SetSearchConditonCookie<T>(string name, T value)
        {
            // ブラウザの上限がだいたい4KBのため。（ブラウザによって差異あり）
            const int cookieLimit = 4000;

            var json = JsonConvert.SerializeObject(value);
            var bin = Encoding.UTF8.GetBytes(json);
            using (var ms = new MemoryStream(bin))
            using (var mso = new MemoryStream())
            {
                using (var zs = new DeflateStream(mso, CompressionMode.Compress)) ms.CopyTo(zs);

                var encoded = Convert.ToBase64String(mso.ToArray()).Replace('+', '-').Replace('/', '_');

                if (Encoding.UTF8.GetByteCount(encoded) > cookieLimit)
                    throw new Exception(string.Format(MessagesResource.LeargeSizeCookie, name, cookieLimit));

                var cookie = new HttpCookie(name, encoded)
                {
                    Expires = DateTime.Now.AddDays(1)
                };
                HttpContext.Current.Response.SetCookie(cookie);
            }
        }
    }
}