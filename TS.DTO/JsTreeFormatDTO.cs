using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class JsTreeFormatDTO
    {
        public string id;
        public string parent;
        public string text;
        public State state;
        public string Message { get; set; }
        public class State
        {
            public bool opened = false;
            public bool disabled = false;
            public bool selected = false;

            public State(bool Opened, bool Disabled, bool Selected)
            {
                opened = Opened;
                disabled = Disabled;
                selected = Selected;
            }
        }
    }
}
