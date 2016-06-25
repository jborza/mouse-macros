using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MouseMacros
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Console.Beep();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int delay = int.Parse(textBox1.Text);
            using (OpenFileDialog od = new OpenFileDialog())
            {
                var result = od.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                currentMacro = MacroSerializer.Load(od.FileName);
                ShowRecordedLength();
                var runner = new MacroRunner(currentMacro);
                runner.MacroPreStartupMilliseconds = delay;
                if (checkBoxLoop.Checked)
                    runner.RunIndefinitely();
                else
                    runner.RunOnce();
            }
        }

        static int hHook = 0;
        Input.HookProc MouseHookProcedure;

        private void buttonRecord_Click(object sender, EventArgs e)
        {
            MouseHookProcedure = new Input.HookProc(MouseHookProc);
            hHook = SetHook(MouseHookProcedure);
            currentMacro = new Macro();
        }

        private static int SetHook(Input.HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return Input.SetWindowsHookEx(Input.WH_MOUSE_LL, proc,
                    Windows.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Input.UnhookWindowsHookEx(hHook);
            MacroSerializer.Save(currentMacro,"macro.xml");
            ShowRecordedLength();
        }

        private void ShowRecordedLength()
        {
            labelLength.Text = currentMacro.TotalLength.ToString();
        }

        public int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Input.MouseHookStruct data = (Input.MouseHookStruct)
                Marshal.PtrToStructure(lParam, typeof(Input.MouseHookStruct));
            var lowLevelData = (Input.MouseHookStructLL)Marshal.PtrToStructure(lParam, typeof(Input.MouseHookStructLL));

            if ((Input.MouseMessages)wParam == Input.MouseMessages.WM_LBUTTONDOWN)
                currentMacro.Add(new MouseDown() { X = data.pt.x, Y = data.pt.y });
            else if ((Input.MouseMessages)wParam == Input.MouseMessages.WM_LBUTTONUP)
                currentMacro.Add(new MouseUp() { X = data.pt.x, Y = data.pt.y });
            else if ((Input.MouseMessages)wParam == Input.MouseMessages.WM_MOUSEWHEEL)
            {
                short mouseWheelDelta = Input.GetMouseWheelDelta(lowLevelData);
                currentMacro.Add(new MouseWheel() { X = data.pt.x, Y = data.pt.y, Delta = mouseWheelDelta });
            }
            return Input.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        Macro currentMacro;
    }


}
