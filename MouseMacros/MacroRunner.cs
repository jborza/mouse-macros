using System;
using System.Threading;

namespace MouseMacros
{
    class MacroRunner
    {
        private readonly Macro macro;

        public MacroRunner(Macro m)
        {
            this.macro = m;
        }

        public int MacroPreStartupMilliseconds = 5000;
        public int MacroPauseBetweenRuns = 1000;

        public void RunIndefinitely()
        {
            Console.Beep(300, 200);
            Thread.Sleep(MacroPreStartupMilliseconds);
            while (true)
            {
                RunOnce();
            }
        }

        public void RunOnce()
        {
            Console.Beep(1000, 200);
            foreach (var click in macro.Actions)
            {
                if (click is WaitAction)
                {
                    var w = click as WaitAction;
                    Thread.Sleep(w.Milliseconds);
                }
                else if (click is MouseDown)
                {
                    var d = click as MouseDown;
                    Input.DoMouseDown(d.X, d.Y);
                }
                else if (click is MouseUp)
                {
                    var u = click as MouseUp;
                    Input.DoMouseUp(u.X, u.Y);
                }
                else if(click is MouseWheel)
                {
                    var w = click as MouseWheel;
                    Input.DoMouseWheelScroll(w.X, w.Y, w.Delta);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Console.Beep(800, 200);
                Thread.Sleep(200);
            }
        }
    }
}
