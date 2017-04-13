using System;
using System.Text;
using MyChy.Frame.Core.Common.Model;

namespace MyChy.Frame.Core.Common.Helper
{
    public class EncryptServerHelper
    {
        /// <summary>
        /// 显示加密后结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="receiptEncrypt"></param>
        /// <returns></returns>
        public static ReceiptEncryptModel ShowEncrypt(object result, string receiptEncrypt)
        {
            var receiptModel = new ReceiptEncryptModel();
            var receipt = StringHelper.Serialize(result);
            var bytes = Encoding.UTF8.GetBytes(receipt);
            receiptModel.Encrypt = Convert.ToBase64String(bytes);
            receiptModel.Sign = SafeSecurityHelper.Sha1(receiptModel.Encrypt + receiptEncrypt).ToLower();
            return receiptModel;
        }

        /// <summary>
        /// 解密返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="encrypt"></param>
        /// <param name="receiptEncrypt"></param>
        /// <returns></returns>
        public static T ResultEncrypt<T>(ReceiptEncryptModel encrypt, string receiptEncrypt)
        {
            if (string.IsNullOrEmpty(encrypt.Encrypt) || string.IsNullOrEmpty(encrypt.Sign)) return default(T);
            try
            {
                encrypt.Encrypt = encrypt.Encrypt.Replace(" ", "+");
                var key = encrypt.Encrypt + receiptEncrypt;
                var sign = SafeSecurityHelper.Sha1(key).ToLower();
                if (encrypt.Sign != sign) return default(T);
                var outputb = Convert.FromBase64String(encrypt.Encrypt);
                var orgStr = Encoding.UTF8.GetString(outputb);
                var receiptresult = StringHelper.Deserialize<T>(orgStr);
                return receiptresult;
            }
            catch (Exception exception)
            {
                LogHelper.Log(exception);
                return default(T);
            }
        }

        /// <summary>
        /// 解密返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static T ResultEncrypt<T>(ReceiptEncryptModel encrypt)
        {
            if (string.IsNullOrEmpty(encrypt.Encrypt)) return default(T);
            encrypt.Encrypt = encrypt.Encrypt.Replace(" ", "+");
            var outputb = Convert.FromBase64String(encrypt.Encrypt);
            var orgStr = Encoding.UTF8.GetString(outputb);
            var receiptresult = StringHelper.Deserialize<T>(orgStr);
            return receiptresult;
        }
    }
}
