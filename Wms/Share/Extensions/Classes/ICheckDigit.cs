namespace Share.Extensions.Classes
{
    /// <summary>
    /// チェックデジットを持つコードのインターフェース
    /// </summary>
    public interface ICheckDigit
    {
        /// <summary>
        /// チェックデジットを取得する
        /// </summary>
        /// <param name="value">対象コード(12桁)</param>
        /// <returns>チェックデジット</returns>
        string Calculate(string value);

        /// <summary>
        /// チェックデジットの検証
        /// </summary>
        /// <param name="value">対象コード(13桁)</param>
        /// <returns>検証結果</returns>
        bool Validate(string value);
    }
}