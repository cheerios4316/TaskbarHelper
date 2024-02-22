# THOT: Tray Helper On Taskbar
## What is it
It's a helper for Windows that lives in your taskbar.
## What it does
For now, it changes wallpaper and has links to folders.
## How to use
Launch the program, then close it.
In your Documents folder, find `thot_data` folder. Within that, there is a `thot_config.ini` file.
Config settings work as follows:
`setting=value`

Settings that accept multiple values use a semicolon **;** as a separator.

### List of available settings

| **Command**      | **Value**                    | **Description**                                                |
|------------------|------------------------------|----------------------------------------------------------------|
| `wallpaper_path` | One path to a folder         | Lets you switch wallpapers with the image files in the folder. |
| `quick_folders`  | One path to a folder or more | Lets you open the chosen folders in explorer.                  |

### Example config file
```
wallpaper_path=C:\Users\JesusHChrist\Documents\Wallpapers
quick_folders=D:\Pirated Games;D:\Pirated Movies
```
