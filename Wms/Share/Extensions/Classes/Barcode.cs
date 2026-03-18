namespace Share.Extensions.Classes
{
    using System;
    using Share.Common.Resources;

    /// <summary>
    /// バーコード情報
    /// </summary>
    public class Barcode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">バーコードの種類</param>
        /// <param name="value">対象コード</param>
        private Barcode(Type type, string value)
        {
            // チェックディジットを付与
            var jancode = new Modulus10Weight3();
            this.Value = value + jancode.Calculate(value);

            this.OriginalValue = value;
        }

        /// <summary>
        /// Barcodeの種類を返す(JAN、コード128など)
        /// </summary>
        public Type BarcodeType { get; private set; }

        /// <summary>
        /// 正しいチェックディジットのバーコードの値を返す
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 入力されたバーコードの値を返す。
        /// </summary>
        public string OriginalValue { get; private set; }

        /// <summary>
        /// 入力されたバーコードの値が正しいかどうかを返す。
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Barcodeクラスのファクトリーメソッド
        /// </summary>
        /// <param name="value">対象コード</param>
        /// <returns>バーコード情報</returns>
        public static Barcode CreateJan(string value)
        {
            // 桁数チェック
            if (value.Length != 12)
                throw new ArgumentException(MessagesResource.JanRegex12);

            return new Barcode(Type.Jan, value);
        }

        /// <summary>
        /// バーコードの種類
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Janコード
            /// </summary>
            Jan
        }
    }
}