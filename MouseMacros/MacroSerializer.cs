using System;
using System.IO;
using System.Xml.Serialization;

namespace MouseMacros
{
    class MacroSerializer
    {
        private static XmlSerializer MakeSerializer()
        {
            return new XmlSerializer(typeof(Macro), new Type[] { typeof(MouseAction), typeof(MouseUp), typeof(MouseDown), typeof(WaitAction), typeof(MouseWheel) });
        }

        public static void Save(Macro macro, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                MakeSerializer().Serialize(fs, macro);
            }
        }

        public static Macro Load(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return MakeSerializer().Deserialize(fs) as Macro;
            }
        }
    }
}
