using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MouseMacros
{
    static class Input
    {
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs,
                                          int cbSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
 CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
 CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode,
IntPtr wParam, IntPtr lParam);

        private static INPUT PrepareInput(int dwFlags)
        {
            INPUT i = new INPUT();
            i.type = INPUT_MOUSE;
            i.mi.dx = 0;
            i.mi.dy = 0;
            i.mi.dwFlags = dwFlags;
            i.mi.dwExtraInfo = IntPtr.Zero;
            i.mi.mouseData = 0;
            i.mi.time = 0;
            return i;
        }

        internal static void DoMouseDown(int x, int y)
        {
            Cursor.Position = new Point(x, y);
            INPUT i = PrepareInput(MOUSEEVENTF_LEFTDOWN);
            SendInput(1, ref i, Marshal.SizeOf(i));
        }

        internal static void DoMouseWheelScroll(int x, int y, short delta)
        {
            Cursor.Position = new Point(x, y);
            INPUT i = PrepareInput(MOUSEEVENTF_WHEEL);
            i.mi.mouseData = delta;
            SendInput(1, ref i, Marshal.SizeOf(i));
        }

        internal static void DoMouseUp(int x, int y)
        {
            Cursor.Position = new Point(x, y);
            INPUT i = PrepareInput(MOUSEEVENTF_LEFTUP);
            SendInput(1, ref i, Marshal.SizeOf(i));
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto,
CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //delegates
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        //mouse event constants
        const int MOUSEEVENTF_LEFTDOWN = 2;
        const int MOUSEEVENTF_LEFTUP = 4;
        const int MOUSEEVENTF_WHEEL = 0x0800;

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        //input type constant
        const int INPUT_MOUSE = 0;
        public const int WH_MOUSE = 7;
        public const int WH_MOUSE_LL = 14;

        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        internal static short GetMouseWheelDelta(MouseHookStructLL lowLevelData)
        {
            return (short)(lowLevelData.mouseData >> 16);
        }

        public struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStructLL
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        public static void DoOneClick(Point location)
        {
            Cursor.Position = location;
            //set up the INPUT struct and fill it for the mouse down
            INPUT i = new INPUT();
            i.type = INPUT_MOUSE;
            i.mi.dx = 0;
            i.mi.dy = 0;
            i.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            i.mi.dwExtraInfo = IntPtr.Zero;
            i.mi.mouseData = 0;
            i.mi.time = 0;
            //send the input 
            SendInput(1, ref i, Marshal.SizeOf(i));
            Thread.Sleep(50);
            //set the INPUT for mouse up and send it
            i = MouseUp(i);
        }

        private static INPUT MouseUp(INPUT i)
        {
            i.mi.dwFlags = MOUSEEVENTF_LEFTUP;
            SendInput(1, ref i, Marshal.SizeOf(i));
            return i;
        }
    }
}
