using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace CSLibrary
{
    internal class NativeRegistry
    {
        internal static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);

        internal enum KeyType
        {
            REG_NONE = 0,
            REG_SZ = 1,
            REG_EXPAND_SZ = 2,
            REG_BINARY = 3,
            REG_DWORD = 4,
            REG_DWORD_LITTLE_ENDIAN = 4,
            REG_DWORD_BIG_ENDIAN = 5,
            REG_LINK = 6,
            REG_MULTI_SZ = 7,
        }
        [DllImport("coredll.dll")]
        internal static extern int RegOpenKeyEx(
           UIntPtr hKey,
            Byte[] lpSubKey,
            uint ulOptions,
            int samDesired,
            out UIntPtr phkResult);
        [DllImport("coredll.dll")]
        internal static extern int RegOpenKeyEx(
           UIntPtr hKey,
            String lpSubKey,
            uint ulOptions,
            int samDesired,
            out UIntPtr phkResult);
        [DllImport("coredll.dll")]
        internal extern static int RegEnumKeyEx(UIntPtr hkey,
            uint index,
            Byte[] lpName,
            ref uint lpcbName,
            IntPtr reserved,
            IntPtr lpClass,
            IntPtr lpcbClass,
            out long lpftLastWriteTime);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int RegQueryValueEx(
            UIntPtr hkey, 
            String lpValueName, 
            IntPtr lpReserved, 
            ref KeyType lpType, 
            Byte[] lpData, 
            ref uint lpcbData);

        [DllImport("coredll.dll")]
        internal static extern int RegCloseKey(UIntPtr hKey);
    }
}
