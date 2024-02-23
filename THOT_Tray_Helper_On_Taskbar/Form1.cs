using System.Diagnostics;
using System.Runtime.InteropServices;

namespace THOT_Tray_Helper_On_Taskbar
{
    public partial class Form1 : Form
    {
        private string documentsPath;
        private string programDataPath;

        private SettingsManager settingsManager;

        public Form1()
        {
            InitializeComponent();

            this.documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.programDataPath = Path.Combine(this.documentsPath, ConfigStrings.PROGRAM_FOLDER);
            this.settingsManager = new SettingsManager(this.programDataPath, ConfigStrings.CONFIG_FILE_NAME);

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

            ToolStripMenuItem[] quickFolderList = ContextFunctions.GenerateQuickFoldersOptionList(quickFolderPathList.Values);

            if (quickFolderList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateQuickFoldersItem(quickFolderList));
            #endregion

            //Quick Web
            #region QUICK WEB

            /**
             * @todo IMPLEMENT QUICK WEB WITH LINKS TO OPEN IN DEFAULT BROWSER
             */

            #endregion

            //Wallpaper
            #region WALLPAPER
            Setting wallpaperPath = this.settingsManager.FetchUserSetting(SettingNames.WALLPAPER_PATH_SETTING);
            ToolStripMenuItem[] wallpaperList = ContextFunctions.GenerateWallpaperOptionList(wallpaperPath.Value);

            if (wallpaperPath.Value != String.Empty && wallpaperList.Length > 0) contextMenuStrip1.Items.Add(ContextFunctions.GenerateWallpaperListItem(wallpaperList));
            #endregion

            //Edit config file
            #region EDIT CONFIG FILE

            string configPath = Path.Combine(this.programDataPath, ConfigStrings.CONFIG_FILE_NAME);
            bool config_exists = File.Exists(configPath);

            if (config_exists)
            {
                contextMenuStrip1.Items.Add(new ToolStripSeparator());
                contextMenuStrip1.Items.Add(Labels.EDIT_CONFIG_TEXT, null, (sender, e) => { Process.Start(new ProcessStartInfo(configPath) { UseShellExecute = true}); });
            }

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