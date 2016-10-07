using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;


namespace ALE.SSISComponentWrap
{
    //Copied from here: http://blogs.msdn.com/b/mattm/archive/2009/08/03/looking-up-ssis-hresult-comexception-errorcode.aspx

    /*Usage:
     *  HResultHelper helper = new HResultHelper();

            try
            {
                CreatePackage();
            }
            catch (COMException ex)
            {
                string errorMessage = helper.GetErrorMessage(ex);
                Console.WriteLine(errorMessage);
            }

            helper.D
     * ispose();
     * */

    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer,
           uint nSize, IntPtr pArguments);

        public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;
        public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;
        public const int FORMAT_MESSAGE_FROM_STRING = 1024;
        public const int FORMAT_MESSAGE_FROM_HMODULE = 2048;
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;
        public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
        public const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 255;
    }

    public class HResultHelper : IDisposable
    {
        const string MessageDLL = @"C:\Program Files\Microsoft SQL Server\100\DTS\Binn\dtsmsg100.dll";

        private IntPtr handle = IntPtr.Zero;
        bool disposed;
        const int FormatMessageFlags = NativeMethods.FORMAT_MESSAGE_ALLOCATE_BUFFER | // FormatMessage will allocate buffer
                                       NativeMethods.FORMAT_MESSAGE_FROM_HMODULE |    // Messages are loaded from specified DLL
                                       NativeMethods.FORMAT_MESSAGE_IGNORE_INSERTS;   // Don't perform place holder substitutions

        public HResultHelper()
        {
            // Load the DLL we'll get the error messages from
            handle = NativeMethods.LoadLibrary(MessageDLL);
            if (handle == IntPtr.Zero)
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, "Couldn't open library: {0}", MessageDLL));
            }
        }

        public string GetErrorMessage(COMException comException)
        {
            return GetErrorMessage((uint)comException.ErrorCode);
        }

        public string GetErrorMessage(uint errorCode)
        {
            IntPtr lpMsgBuf = IntPtr.Zero;
            uint dwChars = NativeMethods.FormatMessage(
                                FormatMessageFlags,
                                handle,                 // handle to error message dll
                                errorCode,              // error code we're looking up
                                0,                      // 0 for default language
                                ref lpMsgBuf,           // message buffer
                                2048,                   // max size of message buffer
                                IntPtr.Zero);           // substitution arguments

            if (dwChars == 0)
            {
                // FormatMessage will set LastError
                throw new Win32Exception();
            }

            string sRet = Marshal.PtrToStringAnsi(lpMsgBuf);

            // Free the buffer
            Marshal.FreeHGlobal(lpMsgBuf);

            return sRet;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                NativeMethods.FreeLibrary(handle);
                handle = IntPtr.Zero;
                disposed = true;
            }
        }

        ~HResultHelper()
        {
            Dispose(false);
        }

     
    }
}
