namespace Share.Common
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Hash utility
    /// </summary>
    public class HashUtil
    {
        /// <summary>
        /// Number of hash byte
        /// </summary>
        private const int HASH_BYTE_COUNT = 20;

        /// <summary>
        /// Number of salt byte
        /// </summary>
        private const int SALT_BYTE_COUNT = 16;

        /// <summary>
        /// Number of Iterations
        /// </summary>
        private const int HASH_ITERATION = 10000;

        /// <summary>
        /// Create SALT
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSalt()
        {
            // Create SALT byte array with fixed byte count
            byte[] saltBytes = new byte[SALT_BYTE_COUNT];

            // Generate SALT
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(saltBytes);
            }

            // Return SALT byte array
            return saltBytes;
        }

        /// <summary>
        /// Create Hash with SALT
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="saltByte">SALT</param>
        /// <returns></returns>
        public static byte[] GenerateSaltedHash(string password, byte[] saltByte)
        {
            // Concat password and salt using PBKDF2 algorithm
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltByte, HASH_ITERATION))
            {
                // Place string in the byte array
                byte[] hash = pbkdf2.GetBytes(HASH_BYTE_COUNT);

                // Create new byte to save Hash and Salt
                byte[] hashBytes = new byte[HASH_BYTE_COUNT + SALT_BYTE_COUNT];

                // Place hash and salt in their respective places
                Array.Copy(saltByte, 0, hashBytes, 0, SALT_BYTE_COUNT);
                Array.Copy(hash, 0, hashBytes, SALT_BYTE_COUNT, HASH_BYTE_COUNT);

                // Convert to a string and return
                return hashBytes;
            }
        }

        /// <summary>
        /// パスワード確認
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <param name="hash"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool VerifyHash(string strPlainPassword, string strHash)
        {
            //bool isValid = true;
            //byte[] hashByte = Convert.FromBase64String(strHash);
            //byte[] saltByte = new byte[SALT_BYTE_COUNT];
            //Array.Copy(hashByte, 0, saltByte, 0, SALT_BYTE_COUNT);

            //// Encode plain password and compare with saved password
            //using (var pbkdf2 = new Rfc2898DeriveBytes(strPlainPassword, saltByte, HASH_ITERATION))
            //{
            //    byte[] hashPassword = pbkdf2.GetBytes(HASH_BYTE_COUNT);

            //    // Byte compare
            //    for (var idx = 0; idx < HASH_BYTE_COUNT; idx++)
            //    {
            //        if (hashByte[SALT_BYTE_COUNT + idx] != hashPassword[idx])
            //        {
            //            // Set invalid
            //            isValid = false;
            //            break;
            //        }
            //    }

            //    // return result
            //    return isValid;
            //}

            // MD5
            if (ComputeHashMd5(strPlainPassword) != strHash)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// MD5ハッシュ値計算
        /// </summary>
        /// <param name="plain"></param>
        /// <returns></returns>
        public static string ComputeHashMd5(string plain)
        {
            byte[] beforeByteArray = System.Text.Encoding.UTF8.GetBytes(plain);
            byte[] afterByteArray;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                afterByteArray = md5.ComputeHash(beforeByteArray);
                md5.Clear();
            }

            var sb = new System.Text.StringBuilder();
            foreach (byte b in afterByteArray)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}