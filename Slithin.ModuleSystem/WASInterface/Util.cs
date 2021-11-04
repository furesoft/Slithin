using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Slithin.ModuleSystem.WASInterface
{
    public static class Util
    {
        public static string FromUtf8(IntPtr nativeString, int size)
        {
            string result = null;

            if (nativeString != IntPtr.Zero)
            {
                var array = new byte[size];
                Marshal.Copy(nativeString, array, 0, size);
                result = Encoding.UTF8.GetString(array, 0, array.Length);
            }

            return result;
        }

        public static byte[] ToUtf8(string sourceText)
        {
            if (sourceText == null)
            {
                return null;
            }

            int nlen = Encoding.UTF8.GetByteCount(sourceText);

            var byteArray = new byte[nlen];
            nlen = Encoding.UTF8.GetBytes(sourceText, 0, sourceText.Length, byteArray, 0);

            return byteArray;
        }

        public static byte[] ToUtf8Z(string sourceText)
        {
            if (sourceText == null)
            {
                return null;
            }

            int nlen = Encoding.UTF8.GetByteCount(sourceText) + 1;

            var byteArray = new byte[nlen];
            byteArray = new byte[nlen];
            nlen = Encoding.UTF8.GetBytes(sourceText, 0, sourceText.Length, byteArray, 0);
            byteArray[nlen] = 0;

            return byteArray;
        }
    }
}