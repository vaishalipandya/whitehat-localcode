using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    /// Cookie class 
    /// </summary>
    public static class CookieUtil
    {
        /// <summary>
        ///     The internet cookie- http only.
        /// </summary>
        private const int InternetCookieHttponly = 0x2000;

        /// <summary>
        ///     The Internet Optionto clear browser session
        /// </summary>
        private const int InternetOptionEndBrowserSession = 42;

        /// <summary>
        ///  Get  Internet  cookie.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieData">The cookie data.</param>
        /// <param name="size">The size.</param>
        /// <param name="flags">The  flags.</param>
        /// <param name="reserved">The  reserved.</param>
        /// <returns></returns>
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            int flags,
            IntPtr reserved);

        /// <summary>
        ///     Gets cookie from URI
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string GetCookieFromUri(Uri uri)
        {
            string cookiedata;

            // Determine the size of the cookie
            int datasize = 8192*16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (
                !InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;

                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null,
                    cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }

            if (cookieData.Length > 0)
            {               
                cookiedata = Convert.ToString(cookieData);
                new Uri(uri.GetLeftPart(UriPartial.Authority));
            }
            else
            {
                cookiedata = string.Empty;
            }

            return cookiedata;
        }

        /// <summary>
        ///     Set option for browser session
        /// </summary>
        /// <param name="internet">The  internet.</param>
        /// <param name="option">The option.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        /// <returns></returns>
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr internet, int option, IntPtr buffer, int bufferLength);

        /// <summary>
        ///     Clears the cookie.
        /// </summary>
        public static void ClearCookie()
        {
            InternetSetOption(IntPtr.Zero, InternetOptionEndBrowserSession, IntPtr.Zero, 0);
        }
    }
}