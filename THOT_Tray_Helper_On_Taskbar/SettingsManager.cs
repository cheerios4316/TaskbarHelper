namespace THOT_Tray_Helper_On_Taskbar
{
    internal class SettingsManager
    {
        private Dictionary<string, Setting> userSettings;

        private string programDataPath, configFileName;

        private List<string> quickLaunchValues;

        public SettingsManager(string programDataPath, string configFileName) 
        {
            this.userSettings = new Dictionary<string, Setting>();
            this.programDataPath = programDataPath;
            this.configFileName = configFileName;
            this.quickLaunchValues = new List<string>();
        }

        public void LoadConfig()
        {
            string configPath = Path.Combine(this.programDataPath, configFileName);
            bool config_exists = File.Exists(configPath);
            string[] configContent = Array.Empty<string>();

            StreamWriter? streamWriter = null;

            if (!config_exists) streamWriter = File.CreateText(configPath);

            configContent = (streamWriter != null) ? this.CreateDefaultConfigFile() : File.ReadAllLines(configPath);

            foreach (string line in configContent)
            {
                string result = this.ManageSettingsLine(line, streamWriter);
                if (result != String.Empty) this.AddComplexSetting(result);
            }

            this.userSettings["quick_launch"] = new Setting("quick_launch", this.quickLaunchValues.ToArray());

            if (streamWriter != null) streamWriter.Dispose();
        }

        private void AddComplexSetting(string line)
        {
            string[] parts = line.Split('+');

            (string key, string value) = (parts[0], parts[1]);

            switch(key)
            {
                case "quick_launch":
                    if (!this.quickLaunchValues.Contains(value)) this.AddUniqueValue(value, quickLaunchValues);
                    break;
            }
        }

        private void AddUniqueValue(string value, List<string> values)
        {
            if (value == String.Empty) return;
            if (values.Contains(value)) return;

            values.Add(value);
        }

        private string ManageSettingsLine(string line, StreamWriter? sw = null)
        {
            if (sw != null) sw.WriteLine(line);

            string[] parts = line.Split('=');
            if (parts.Length == 2) this.userSettings[parts[0]] = new Setting(parts[0], parts[1]);

            if (parts.Length == 1) return line;

            return String.Empty;
        }

        private string[] CreateDefaultConfigFile()
        {
            List<string> res = new List<string>();

            string wallpaperPath = Path.Combine(this.programDataPath, "wallpapers");

            res.Add("wallpaper_path=" + wallpaperPath);
            if (!Directory.Exists(wallpaperPath)) Directory.CreateDirectory(wallpaperPath);
            res.Add("quick_folders=");
            res.Add("quick_launch+");

            return res.ToArray();
        }

        public Setting FetchUserSetting(string setting)
        {
            this.userSettings.TryGetValue(setting, out Setting? result);

            return result ?? new Setting(setting, "");
        }
    }
}
