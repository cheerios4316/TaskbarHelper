namespace THOT_Tray_Helper_On_Taskbar
{
    internal class SettingsManager
    {
        private Dictionary<string, Setting> userSettings;

        private string programDataPath, configFileName;

        public SettingsManager(string programDataPath, string configFileName) 
        {
            this.userSettings = new Dictionary<string, Setting>();
            this.programDataPath = programDataPath;
            this.configFileName = configFileName;
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
                if (result !== String.Empty) this.AddComplexSetting
                        /**
                         * todo
                         * fai funzione addcomplexsetting
                         * questa funzione deve dividere per + e fare uno switch sul lato sinistro della stringa
                         * se lato_sx == unacertaopzione allora deve aggiungere a un dato array di stringhe un elemento = lato_dx
                         * 
                         * case('una_opzione'):
                         *     this.una_opzione_values.Add(lato_dx)
                         *     break;
                         * case('altra opzione')... etc...
                         * 
                         * poi dopo la fine di questo ciclo qui creo un setting per ciascuno di quegli array:
                         * 
                         * this.userSettings['una_opzione'] = new Setting('una_opzione', this.una_opzione_values);
                         * this.userSettings['altra_opzione'] = new Setting('altra_opzione', this.altra_opzione_values);
                         */
            }

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

        private string[] CreateDefaultConfigFile()
        {
            List<string> res = new List<string>();

            string wallpaperPath = Path.Combine(this.programDataPath, "wallpapers");

            res.Add("wallpaper_path=" + wallpaperPath);
            if (!Directory.Exists(wallpaperPath)) Directory.CreateDirectory(wallpaperPath);
            res.Add("quick_folders=");

            return res.ToArray();
        }

        public Setting FetchUserSetting(string setting)
        {
            this.userSettings.TryGetValue(setting, out Setting? result);

            return result ?? new Setting(setting, "");
        }
    }
}
