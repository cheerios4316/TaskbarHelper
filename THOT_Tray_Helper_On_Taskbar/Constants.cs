using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THOT_Tray_Helper_On_Taskbar
{
    public static class SettingNames
    {
        public const string QUICK_LAUNCH_SETTING = "quick_launch";
        public const string QUICK_FOLDERS_SETTING = "quick_folders";
        public const string WALLPAPER_PATH_SETTING = "wallpaper_path";
    }

    public static class Labels
    {
        public const string TASKBAR_ICON_TEXT = "Tray Helper On Taskbar";
        public const string EXIT_TEXT = "Exit";
        public const string CHANGE_WALLPAPER = "Change Wallpaper";
        public const string QUICK_FOLDERS = "Quick Folders";
        public const string QUICK_LAUNCH = "Quick Launch";
        public const string EDIT_CONFIG_TEXT = "Edit config file";
    }

    public static class ConfigStrings
    {
        public const string PROGRAM_FOLDER = "thot_data";
        public const string CONFIG_FILE_NAME = "thot_config.ini";
        public const string DEFAULT_WALLPAPER_FOLDER = "wallpapers";
    }

    public static class ProgramData
    {
        public static readonly string[] VALID_QUICKLAUNCH_TYPES = { "exe", "bat" };
    }
}
