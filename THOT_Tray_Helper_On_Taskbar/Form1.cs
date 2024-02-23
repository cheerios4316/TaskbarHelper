using System.Runtime.InteropServices;

namespace THOT_Tray_Helper_On_Taskbar
{
    public partial class Form1 : Form
    {
        private string documentsPath;
        private string programDataPath;
        private const string PROGRAM_FOLDER_NAME = ConfigStrings.PROGRAM_FOLDER;
        private const string CONFIG_FILE_NAME = ConfigStrings.CONFIG_FILE_NAME;

        private SettingsManager settingsManager;

        public Form1()
        {
            InitializeComponent();

            this.documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
            //Quick launch
            #region QUICK LAUNCH
            Setting quickLaunchPathList = this.settingsManager.FetchUserSetting(SettingNames.QUICK_LAUNCH_SETTING);

            ToolStripMenuItem[] quickLaunchList = ContextFunctions.GenerateQuickLaunchOptionList(quickLaunchPathList.Values);

            if (quickLaunchList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateQuickLaunchItem(quickLaunchList));
            #endregion

            //Quick folders
            #region QUICK FOLDERS
            Setting quickFolderPathList = this.settingsManager.FetchUserSetting(SettingNames.QUICK_FOLDERS_SETTING);

            ToolStripMenuItem[] quickFolderList = ContextFunctions.GenerateQuickFoldersOptionList(quickFolderPathList.Value);

            if (quickFolderList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateQuickFoldersItem(quickFolderList));
            #endregion

            //Wallpaper
            #region WALLPAPER
            Setting wallpaperPath = this.settingsManager.FetchUserSetting(SettingNames.WALLPAPER_PATH_SETTING);
            ToolStripMenuItem[] wallpaperList = ContextFunctions.GenerateWallpaperOptionList(wallpaperPath.Value);

            if (wallpaperPath.Value != String.Empty && wallpaperList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateWallpaperListItem(wallpaperList));
            #endregion

            //Exit
            #region EXIT
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            contextMenuStrip1.Items.Add(Labels.EXIT_TEXT, null, (sender, e) => { Application.Exit(); });
            #endregion

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = Labels.TASKBAR_ICON_TEXT;
        }
    }
}