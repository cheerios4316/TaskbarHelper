using System.Runtime.InteropServices;

namespace THOT_Tray_Helper_On_Taskbar
{
    public partial class Form1 : Form
    {
        private string documentsPath;
        private string programDataPath;
        private const string PROGRAM_FOLDER_NAME = "thot_data";
        private const string CONFIG_FILE_NAME = "thot_config.ini";
        private Dictionary<string, string> userSettings;

        private SettingsManager settingsManager;

        public Form1()
        {
            InitializeComponent();

            this.documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.userSettings = new Dictionary<string, string>();
            this.programDataPath = Path.Combine(this.documentsPath, PROGRAM_FOLDER_NAME);
            this.settingsManager = new SettingsManager(this.programDataPath, CONFIG_FILE_NAME);

            this.ApplySettings();

            this.settingsManager.LoadConfig();

            this.LoadContextMenuOptions();
        }

        private void ApplySettings()
        {
            this.ShowInTaskbar = false;
            this.Opacity = 0; // Makes the form transparent
            this.FormBorderStyle = FormBorderStyle.None; // Removes the border and title bar
            this.Load += (sender, e) => { this.Hide(); }; // Hides the form when it's loaded

            Directory.CreateDirectory(this.programDataPath);
        }

        private void LoadContextMenuOptions()
        {
            //Wallpaper
            Setting wallpaperPath = this.settingsManager.FetchUserSetting("wallpaper_path");
            ToolStripMenuItem[] wallpaperList = ContextFunctions.GenerateWallpaperOptionList(wallpaperPath.Value);

            if(wallpaperPath.Value != "" && wallpaperList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateWallpaperListItem(wallpaperList));

            //Quick folders
            Setting quickFolderPathList = this.settingsManager.FetchUserSetting("quick_folders");
            ToolStripMenuItem[] quickFolderList = ContextFunctions.GenerateQuickFoldersOptionList(quickFolderPathList.Value);

            if (quickFolderList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateQuickFoldersItem(quickFolderList));

            //Quick launch
            Setting quickLaunchPathList = this.settingsManager.FetchUserSetting("quick_launch");
            ToolStripMenuItem[] quickLaunchList = ContextFunctions.GenerateQuickLaunchOptionList(quickLaunchPathList.Values);

            if (quickLaunchList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateQuickLaunchItem(quickLaunchList));

            //Exit
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            contextMenuStrip1.Items.Add("Exit", null, (sender, e) => { Application.Exit(); });

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "Tray Helper On Taskbar";
        }
    }
}