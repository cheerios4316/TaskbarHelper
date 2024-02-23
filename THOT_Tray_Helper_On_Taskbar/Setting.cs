using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THOT_Tray_Helper_On_Taskbar
{
    internal class Setting
    {
        private string key, value;
        private string[] values;

        public Setting(string key, string value)
        {
            this.key = key;
            this.value = value;
            this.values = Array.Empty<string>();
        }

        public Setting(string key, string[] values)
        {
            this.key = key;
            this.values = values;
            this.value = String.Empty;
        }

        public string Key
        {
            get { return this.key; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string[] Values
        {
            get { return this.values; }
            set { this.values = value; }
        }
    }
}
