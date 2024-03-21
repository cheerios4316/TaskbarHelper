# THOT: Tray Helper On Taskbar
## What it is
It's a helper for Windows that lives in your taskbar.

## What it does
It allows you to change your wallpaper on the fly and fire up your favorite programs and folders at a moment's notice.

## How to use
Launch the program, then close it.
In your Documents folder, find `thot_data` folder. Within that, there is a `thot_config.ini` file.
Config settings work as follows:
`setting=value`

Complex (or cumulative) settings use the `+` sign instead of `=`:
`cumulative+value`
You can add multiple values to a cumulative settings (example at the end of README).

The config file can also be accessed and edited through your default text editor by right clicking the icon on the taskbar and choosing `Edit config file`.
After editing the config file, the program must be restarted in order for the modifications to take place.

### List of available settings

| **Command**      | **Value**                                                                                              | **Description**                                                | **Type**   |
|------------------|--------------------------------------------------------------------------------------------------------|----------------------------------------------------------------|------------|
| `wallpaper_path` | One path to a folder                                                                                   | Lets you switch wallpapers with the image files in the folder. | Normal     |
| `quick_folders`  | A path to a folder                                                                                     | Lets you open the chosen folders in explorer.                  | Cumulative |
| `quick_launch`   | Either a path to a valid file (or a name, followed by a semicolon, followed by a path to a valid file) | Launches the chosen file.                                      | Cumulative |
| `quick_link`     | Same thing as `quick_launch`, but with an URL instead of a path to a file                              | Opens the chosen URL in the default web browser.               |            |


File types valid for `quick_launch`: `.bat`, `.exe`

File types valid for wallpaper files: `png`, `jpg`, `jpeg`, `bmp`, `dib`, `tif`, `tiff`, `jfif`, `jpe`, `gif`, `heif`, `heic`, `webp`


### Example config file
```
wallpaper_path=C:\Users\JesusHChrist\Documents\Wallpapers

quick_folders+D:\Pirated Games
quick_folders+D:\Pirated Movies

quick_launch+C:\Program Files\Cracked_Photoshop\Photoshop.exe
quick_launch+TrayHelper;D:\CShartProjects\TrayHelper.exe
```
