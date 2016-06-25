using System;

namespace MouseMacros
{
    [Serializable]
    public abstract class Action
    {

    }

    [Serializable]
    public class WaitAction : Action
    {
        public int Milliseconds;
    }

    [Serializable]
    public class MouseWheel : MouseAction
    {
        public short Delta;
        public int X;
        public int Y;
    }

    [Serializable]
    public abstract class MouseAction : Action
    {
        public int X;
        public int Y;
    }

    [Serializable]
    public class MouseUp : MouseAction { }

    [Serializable]
    public class MouseDown : MouseAction { }
}
