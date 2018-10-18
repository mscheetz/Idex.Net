using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Idex.Net.Core
{
    public static class Security
    {
        /// <summary>
        /// SecureString to string
        /// </summary>
        /// <param name="secureString">SecureString to convert</param>
        /// <returns>Unsecure string</returns>
        public static string ToUnsecureString(this SecureString secureString)
        {
            if (secureString == null) throw new ArgumentNullException("secureString");

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// String to SecureString
        /// </summary>
        /// <param name="unsecureString">string to convert</param>
        /// <returns>SecureString of unsecure string</returns>
        public static SecureString ToSecureString(this string unsecureString)
        {
            if (unsecureString == null) throw new ArgumentNullException("unsecureString");

            return unsecureString.Aggregate(new SecureString(), AppendChar, MakeReadOnly);
        }

        private static SecureString MakeReadOnly(SecureString ss)
        {
            ss.MakeReadOnly();
            return ss;
        }

        private static SecureString AppendChar(SecureString ss, char c)
        {
            ss.AppendChar(c);
            return ss;
        }

        public static string SignMessage(Dictionary<string, object> message)
        {
            return string.Empty;
        }
    }
}
