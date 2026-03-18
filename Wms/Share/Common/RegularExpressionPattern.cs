namespace Share.Common
{
    /// <summary>
    /// 正規表現パターン
    /// </summary>
    public class RegularExpressionPattern
    {
        /// <summary>
        /// 半角英数字のみ
        /// </summary>
        public const string Alphanumeric = @"[a-zA-Z0-9]+";

        /// <summary>
        /// 大文字の半角英字のみ
        /// </summary>
        public const string UpperAlpha = @"[A-Z]+";

        /// <summary>
        /// ID(半角英数字記号)
        /// </summary>
        public const string Id = @"^[a-zA-Z0-9-/:-@\[-\`\{-\~]([a-zA-Z0-9-/:-@\[-\`\{-\~ ]+|)";
    }
}