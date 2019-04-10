using System;
using System.Text;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    /// Base64 Utility
    /// </summary>
    public static class Bade64EncodeDecode
    {
        /// <summary>
        ///     Decodes code from Base64 Encode.
        /// </summary>
        /// <param name="encodedData">The encoded data.</param>
        /// <returns></returns>
        public static string Base64Decode(string encodedData)
        {
            UTF8Encoding utfEncoding = new UTF8Encoding();
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            string decodedData = utfEncoding.GetString(encodedDataAsBytes);
            return decodedData;
        }

        /// <summary>
        ///     Encodes the data to Base64.
        /// </summary>
        /// <param name="dataToEncode">To data To Encode.</param>
        /// <returns></returns>
        public static string Base64Endoce(string dataToEncode)
        {
            UTF8Encoding utfEncoding = new UTF8Encoding();
            byte[] toEncodeAsBytes = utfEncoding.GetBytes(dataToEncode);
            string encodedData = Convert.ToBase64String(toEncodeAsBytes);
            return encodedData;
        }
    }
}