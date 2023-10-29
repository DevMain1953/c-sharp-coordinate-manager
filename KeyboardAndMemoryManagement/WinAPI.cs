using System;
using System.Runtime.InteropServices;

namespace WinAPI
{
    public class WindowsAPI
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        protected static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("User32.dll")]
        protected static extern short GetAsyncKeyState(System.Int32 vKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool CloseHandle(IntPtr hObject);
    }
}
