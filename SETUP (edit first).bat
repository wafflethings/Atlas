@echo off

:: Set this to your ULTRAKILL path.
set UKPATH="A:\SteamLibrary\steamapps\common\ULTRAKILL"
:: Set this to where your Atlas folder is.
set PATH="C:\Users\mkols\source\repos\Atlas"

:: YOU NEED ADMIN PERMISSIONS TO CREATE A SYMLINK.
mklink /D %PATH%"\Atlas_Assets\Assets\Common" %UKPATH%"\Magenta\Common"

pause