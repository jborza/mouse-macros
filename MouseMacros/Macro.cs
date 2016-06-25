using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MouseMacros
{
    [Serializable]
    public class Macro
    {
        public List<Action> Actions;
        private DateTime LastActionTime;


        public Macro()
        {
            Actions = new List<Action>();
        }

        public void Add(MouseAction mouseAction)
        {
            if (LastActionTime == DateTime.MinValue)
            {
                LastActionTime = DateTime.UtcNow;
            }
            var now = DateTime.UtcNow;
            var timespan = now - LastActionTime;
            LastActionTime = now;
            var millis = (int)timespan.TotalMilliseconds;
            Actions.Add(new WaitAction() { Milliseconds = millis });
            Actions.Add(mouseAction);
        }

        public TimeSpan TotalLength
        {
            get
            {
                double waitMs = Actions.OfType<WaitAction>().Sum(a => a.Milliseconds);
                return TimeSpan.FromMilliseconds(waitMs);
            }
        }
    }
}
