using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace THOT_Tray_Helper_On_Taskbar
{
    internal static class ContextFunctions
    {
        #region WALLPAPER CHANGER

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private static void SetDesktopWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        #endregion

        #region OPTION LIST GENERATORS

        public static ToolStripMenuItem[] GenerateWallpaperOptionList(string inputPath)
        {
            if(!Directory.Exists(inputPath)) return GenerateEmptyItemList();

            string[] pathList = Directory.GetFiles(inputPath);

            if (pathList.Length == 0) return Array.Empty<ToolStripMenuItem>();

            List<ToolStripMenuItem> res = new List<ToolStripMenuItem>();
            int k = 0;
            foreach(string path in pathList)
            {
                string[] pathParts = path.Split('\\');

                string fileName = String.Join('.', pathParts.Last().Split('.').SkipLast(1));
                string fileType = pathParts.Last().Split('.').Last();

                string[] types = ProgramData.VALID_WALLPAPER_TYPES;

                if (!ProgramData.VALID_WALLPAPER_TYPES.Contains(fileType.ToLower())) continue;

                res.Add(new ToolStripMenuItem(fileName, null, (sender, e) => { SetDesktopWallpaper(path); }));
                k++;
            }

            return res.ToArray();
        }

        public static ToolStripMenuItem[] GenerateQuickFoldersOptionList(string[] paths)
        {
            if(paths.Length == 0) return GenerateEmptyItemList();

            List<ToolStripMenuItem> res = new List<ToolStripMenuItem>();

            int k = 0;
            foreach(string path in paths)
            {
                string[] pathParts = path.Split('\\');
                string folderName = pathParts.Last();

                if(Directory.Exists(path)) res.Add(new ToolStripMenuItem((++k).ToString() + ". " + folderName, null, (sender, e) => { Process.Start("explorer.exe", path); }));
            }

            return res.ToArray();
        }

        public static ToolStripMenuItem[] GenerateQuickLaunchOptionList(string[] values)
        {
            if (values.Length == 0) return GenerateEmptyItemList();

            List<ToolStripMenuItem> res = new List<ToolStripMenuItem>();

            int k = 0;
            foreach(string value in values)
            {
                string path = "";
                string labelText = "";
                string[] valueParts = value.Split(';');

                if (valueParts.Length == 1) path = value;
                if (valueParts.Length == 2)
                {
                    path = valueParts[1];
                    labelText = valueParts[0];
                }

                string[] pathParts = path.Split('\\');
                string fullFileName = pathParts.Last();
                string fileName = String.Join('.', fullFileName.Split('.').SkipLast(1));
                string fileExtension = fullFileName.Split('.').Last();

                bool ex = File.Exists(path);

                if (!File.Exists(path)) continue;
                if (!ProgramData.VALID_QUICKLAUNCH_TYPES.Contains(fileExtension.ToLower())) continue;

                string displayName = (labelText != String.Empty) ? labelText : fileName;

                res.Add(new ToolStripMenuItem((++k).ToString() + ". " + displayName, null, (sender, e) => { Process.Start(path); }));
            }

            return res.ToArray();
        }

        public static ToolStripMenuItem[] GenerateQuickLinkOptionList(string[] values)
        {
            if (values.Length == 0) return GenerateEmptyItemList();

            List<ToolStripMenuItem> res = new List<ToolStripMenuItem>();

            int k = 0;
            foreach(string value in values)
            {
                string link = String.Empty;
                string labelText = String.Empty;

                string[] valueParts = value.Split(';');

                if (valueParts.Length == 1) link = SanitizeUrl(value);
                if (valueParts.Length == 2)
                {
                    link = SanitizeUrl(valueParts[1]);
                    labelText = valueParts[0];
                }

                if (link == String.Empty) continue;

                string displayName = (labelText != String.Empty) ? labelText : ExtractDomain(link);

                res.Add(new ToolStripMenuItem((++k).ToString() + ". " + displayName, null, (sender, e) => { Process.Start(new ProcessStartInfo(link) { UseShellExecute = true }); }));
            }

            return res.ToArray();
        }

        public static ToolStripMenuItem[] GenerateEmptyItemList()
        {
            return Array.Empty<ToolStripMenuItem>();
        }

        #endregion

        #region MAIN MENU ITEM GENERATORS

        public static ToolStripMenuItem GenerateWallpaperListItem(ToolStripMenuItem[] list)
        {
            var submenu = new ToolStripMenuItem(Labels.CHANGE_WALLPAPER);

            ContextFunctions.AddMultipleItems(list, submenu);

            return submenu;
        }

        public static ToolStripMenuItem GenerateQuickFoldersItem(ToolStripMenuItem[] list)
        {
            var submenu = new ToolStripMenuItem(Labels.QUICK_FOLDERS);

            ContextFunctions.AddMultipleItems(list, submenu);

            return submenu;
        }

        public static ToolStripMenuItem GenerateQuickLaunchItem(ToolStripMenuItem[] list)
        {
            var submenu = new ToolStripMenuItem(Labels.QUICK_LAUNCH);

            ContextFunctions.AddMultipleItems(list, submenu);

            return submenu;
        }

        public static ToolStripMenuItem GenerateQuickLinkItem(ToolStripMenuItem[] list)
        {
            var submenu = new ToolStripMenuItem(Labels.QUICK_LINK);

            ContextFunctions.AddMultipleItems(list, submenu);

            return submenu;
        }

        #endregion

        public static void AddMultipleItems(ToolStripMenuItem[] list, ToolStripMenuItem target)
        {
            foreach (var item in list)
            {
                target.DropDownItems.Add(item);
            }
        }

        private static string SanitizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            if (Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase)) return url.EndsWith("/") ? url : $"{url}/";

            return "https://" + url + "/";
        }

        public static string ExtractDomain(string url)
        {
            Regex regex = new Regex(@"https?:\/\/(.*?)(\/|$)");
            Match match = regex.Match(url);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
    }
}
