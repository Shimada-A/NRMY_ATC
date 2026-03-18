namespace Share.Extensions.Classes
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Share.Common.Resources;

    /// <summary>
    /// モジュラス10ウェイト3
    /// </summary>
    public class Modulus10Weight3 : ICheckDigit
    {
        #region "チェックデジットを取得する"

        /// <summary>
        /// チェックデジットを取得する
        /// </summary>
        /// <param name="value">対象コード(12桁)</param>
        /// <returns>チェックデジット</returns>
        public string Calculate(string value)
        {
            // 数値チェック
            if (Regex.IsMatch(value, "[^0-9]"))
            {
                throw new ArgumentException(MessagesResource.CodeNumberInValid);
            }

            // 1の位から10の位の順に奇数偶数を決定するため逆順に並べ替える(モジュラス10ウェイト3の仕様を参照)
            var source = value.ToCharArray().Reverse().Select((text, index) => new { Text = text, Index = index + 1 }).ToList();
            int total = 0;

            foreach (var s in source)
            {
                if (s.Index % 2 == 0)
                {
                    // 偶数は1倍の合計
                    total += int.Parse(s.Text.ToString());
                }
                else
                {
                    // 奇数は3倍した結果の合計
                    total += int.Parse(s.Text.ToString()) * 3;
                }
            }

            // 合計を10で割った余りを10から引く
            int checkDigit = 10 - (total % 10);
            if (checkDigit == 10)
            {
                checkDigit = 0;
            }

            return checkDigit.ToString();
        }

        #endregion "チェックデジットを取得する"

        /// <summary>
        /// チェックデジットの検証
        /// </summary>
        /// <param name="value">対象コード(13桁)</param>
        /// <returns>検証結果(true/false)</returns>
        public bool Validate(string value)
        {
            var checkDigit = value.Substring(value.Length - 1);
            if (checkDigit == this.Calculate(value.Substring(0, value.Length - 1)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}