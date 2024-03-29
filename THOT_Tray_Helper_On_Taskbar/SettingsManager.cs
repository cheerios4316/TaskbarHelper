﻿namespace THOT_Tray_Helper_On_Taskbar
{
    internal class SettingsManager
    {
        private Dictionary<string, Setting> userSettings;

        private string programDataPath, configFileName;

        private List<string> quickLaunchValues;
        private List<string> quickFolderValues;
        private List<string> quickLinkValues;

        public SettingsManager(string programDataPath, string configFileName) 
        {
            this.userSettings = new Dictionary<string, Setting>();
            this.programDataPath = programDataPath;
            this.configFileName = configFileName;

            this.quickLaunchValues = new List<string>();
            this.quickFolderValues = new List<string>();
            this.quickLinkValues = new List<string>();
        }

        #region CONFIG LOADER

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

            /**
             * Create cumulative settings
             */
            this.userSettings[SettingNames.QUICK_LAUNCH_SETTING] = new Setting(SettingNames.QUICK_LAUNCH_SETTING, this.quickLaunchValues.ToArray());
            this.userSettings[SettingNames.QUICK_FOLDERS_SETTING] = new Setting(SettingNames.QUICK_FOLDERS_SETTING, this.quickFolderValues.ToArray());
            this.userSettings[SettingNames.QUICK_LINK_SETTING] = new Setting(SettingNames.QUICK_LINK_SETTING, this.quickLinkValues.ToArray());

            if (streamWriter != null) streamWriter.Dispose();
        }

        private string ManageSettingsLine(string line, StreamWriter? sw = null)
        {
            if (sw != null) sw.WriteLine(line);

            string[] parts = line.Split('=');
            if (parts.Length == 2) this.userSettings[parts[0]] = new Setting(parts[0], parts[1]);

            if (parts.Length == 1) return line;

            return String.Empty;
        }

        private void AddComplexSetting(string line)
        {
            if (line[0] == '#') return;

            string[] parts = line.Split('+');

            (string key, string value) = (parts[0], parts[1]);

            switch (key)
            {
                case SettingNames.QUICK_LAUNCH_SETTING:
                    this.AddUniqueValue(value, quickLaunchValues);
                    break;
                case SettingNames.QUICK_FOLDERS_SETTING:
                    this.AddUniqueValue(value, quickFolderValues);
                    break;
                case SettingNames.QUICK_LINK_SETTING:
                    this.AddUniqueValue(value, quickLinkValues);
                    break;
            }
        }

        #endregion

        private string[] CreateDefaultConfigFile()
        {
            List<string> res = new List<string>();

            string wallpaperPath = Path.Combine(this.programDataPath, ConfigStrings.DEFAULT_WALLPAPER_FOLDER);

            foreach(string line in ProgramData.DEFAULT_CONFIG_CONTENT)
            {
                string parsed = line.Replace(Placeholders.WALLPAPER_PATH, wallpaperPath);
                res.Add(parsed);
            }

            return res.ToArray();
        }

        public Setting FetchUserSetting(string setting)
        {
            this.userSettings.TryGetValue(setting, out Setting? result);

            return result ?? new Setting(setting, String.Empty);
        }

        private void AddUniqueValue(string value, List<string> values)
        {
            if (value == String.Empty) return;
            if (values.Contains(value)) return;

            values.Add(value);
        }
    }
}
