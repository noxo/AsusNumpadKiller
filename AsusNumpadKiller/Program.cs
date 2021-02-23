using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AsusNumpadKiller
{
    // https://docs.microsoft.com/en-us/archive/blogs/toub/low-level-keyboard-hook-in-c
    class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int KEYCODE_NUMLOCK = 144;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void Main(string[] args)
        {
            // https://stackoverflow.com/questions/3168782/visual-studio-installer-how-to-launch-app-at-end-of-installer
            if (args.Length == 1 && args[0] == "INSTALLER")
            {
                Process.Start(Application.ExecutablePath); 
                return;
            }
            else
            {
                // turn numlock off first, before start consuming numlock keyevents
                TurnNumlockOffIfOn();
                _hookID = SetHook(_proc);
                Application.Run();
                UnhookWindowsHookEx(_hookID);
            }
        }
        
        private static void TurnNumlockOffIfOn()
        {
            short keystate = GetKeyState(KEYCODE_NUMLOCK);
            if (keystate == 1)
            {
                keybd_event(KEYCODE_NUMLOCK, 1, 0, 0);
                keybd_event(KEYCODE_NUMLOCK, 0, KEYEVENTF_KEYUP, 0);
            }
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == KEYCODE_NUMLOCK)
                    return (IntPtr)1;
            }
            return CallNextHookEx(_hookID, nCode, wParam, IntPtr.Zero);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern void keybd_event(int vk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);
    }

}
