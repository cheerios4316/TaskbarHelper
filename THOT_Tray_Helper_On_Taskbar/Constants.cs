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
        public const string QUICK_LINK_SETTING = "quick_link";
    }

    public static class Labels
    {
        public const string TASKBAR_ICON_TEXT = "Tray Helper On Taskbar";
        public const string EXIT_TEXT = "Exit";
        public const string CHANGE_WALLPAPER = "Change Wallpaper";
        public const string QUICK_FOLDERS = "Quick Folders";
        public const string QUICK_LAUNCH = "Quick Launch";
        public const string QUICK_LINK = "Quick Links";
        public const string EDIT_CONFIG_TEXT = "Edit config file";
    }

    public static class ConfigStrings
    {
        public const string PROGRAM_FOLDER = "thot_data";
        public const string CONFIG_FILE_NAME = "thot_config.ini";
        public const string DEFAULT_WALLPAPER_FOLDER = "wallpapers";
    }

    public static class Placeholders
    {
        public const string WALLPAPER_PATH = "{WallpaperPathPlaceholder}";
    }

    public static class ProgramData
    {
        public static readonly string[] VALID_QUICKLAUNCH_TYPES = { "exe", "bat" };
        public static readonly string[] VALID_WALLPAPER_TYPES = { "png", "jpg", "jpeg", "bmp", "dib", "tif", "tiff", "jfif", "jpe", "gif", "heif", "heic", "webp" };

        public static readonly string[] DEFAULT_CONFIG_CONTENT =
        {
            "# Tray Helper On Taskbar",
            "# Made by E. Cheerios",
            "# https://github.com/cheerios4316/TaskbarHelper/",
            "",
            "# Read README.md on GitHub for more detailed instructions.",
            "",
            "# Quick Launch Settings (takes .bat, .exe)",
            "# Add Quick Launch programs in two possible ways:",
            "#\t\tquick_launch+program_name;path\\to\\program}",
            "#\t\tquick_launch+path\\to\\program",
            "",
            "quick_launch+",
            "",
            "# Quick Link Settings",
            "# Add Quick Links like Quick Launch but using web URLs",
            "# instead of Explorer paths.",
            "",
            "quick_link+",
            "",
            "# Quick Folder Settings",
            "# Add Quick Folders like this:",
            "#\t\tquick_folders+path\\to\\folder",
            "",
            "quick_folders+",
            "",
            "# Wallpaper Path",
            "# Set a Wallpaper folder path like this:",
            "#\t\twallpaper_path=path\\to\\folder",
            "",
            "wallpaper_path=" + Placeholders.WALLPAPER_PATH
        };
    }
}
