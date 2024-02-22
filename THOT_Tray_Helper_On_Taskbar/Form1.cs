using System.Runtime.InteropServices;

namespace THOT_Tray_Helper_On_Taskbar
{
    public partial class Form1 : Form
    {
        private string documentsPath;
        private string programDataPath;
        private const string PROGRAM_FOLDER_NAME = "thot_data";
        private const string configFileName = "thot_config.ini";
        private Dictionary<string, string> userSettings;

        public Form1()
        {
            InitializeComponent();

            this.documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.userSettings = new Dictionary<string, string>();
            this.programDataPath = Path.Combine(this.documentsPath, PROGRAM_FOLDER_NAME);

            this.applySettings();

            this.handleConfig();

            this.loadContextMenuOptions();
        }

        private void applySettings()
        {
            this.ShowInTaskbar = false;
            this.Opacity = 0; // Makes the form transparent
            this.FormBorderStyle = FormBorderStyle.None; // Removes the border and title bar
            this.Load += (sender, e) => { this.Hide(); }; // Hides the form when it's loaded

            Directory.CreateDirectory(this.programDataPath);
        }

        private void handleConfig()
        {
            string configPath = Path.Combine(this.programDataPath, configFileName);
            bool config_exists = File.Exists(configPath);
            string[] configContent = Array.Empty<string>();

            StreamWriter? streamWriter = null;

            if (!config_exists) streamWriter = File.CreateText(configPath);

            configContent = (streamWriter != null) ? this.CreateDefaultConfigFile() : File.ReadAllLines(configPath);

            foreach(string line in configContent)
            {
                this.manageSettingsLine(line, streamWriter);
            }

            if (streamWriter != null) streamWriter.Dispose();
        }

        private void manageSettingsLine(string line, StreamWriter? sw = null)
        {
            if(sw != null) sw.WriteLine(line); 

            string[] parts = line.Split('=');
            if (parts.Length == 2) this.userSettings[parts[0]] = parts[1];
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

        private string FetchUserSetting(string setting)
        {
            this.userSettings.TryGetValue(setting, out string? result);

            return result ?? "";
        }

        private void loadContextMenuOptions()
        {
            //Wallpaper
            string wallpaperPath = FetchUserSetting("wallpaper_path");
            ToolStripMenuItem[] wallpaperList = ContextFunctions.GenerateWallpaperOptionList(wallpaperPath);

            if(wallpaperPath != "" && wallpaperList.Length > 0) contextMenuStrip1.Items.Add(this.GenerateWallpaperListItem(wallpaperList));

            //Quick folders
            string quickFolderPathList = FetchUserSetting("quick_folders");
            ToolStripMenuItem[] quickFolderList = ContextFunctions.GenerateQuickFoldersOptionList(quickFolderPathList);

            if (quickFolderList.Length > 0) contextMenuStrip1.Items.Add(this.GenerateQuickFoldersItem(quickFolderList));

            //Exit
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            contextMenuStrip1.Items.Add("Exit", null, (sender, e) => { Application.Exit(); });

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "Tray Helper On Taskbar";
        }

        private ToolStripMenuItem GenerateWallpaperListItem(ToolStripMenuItem[] list)
        {
            var submenu = new ToolStripMenuItem("Change Wallpaper");

            ContextFunctions.AddMultipleItems(list, submenu);

            return submenu;
        }

        private ToolStripMenuItem GenerateQuickFoldersItem(ToolStripMenuItem[] list)
        {
            var submenu = new ToolStripMenuItem("Quick Folders");

            ContextFunctions.AddMultipleItems(list, submenu);

            return submenu;
        }
    }
}