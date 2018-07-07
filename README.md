# XCI Organizer

[Original Reddit Release](https://www.reddit.com/r/SwitchHacks/comments/8vma9o/xci_organizer_v001/)

Manage your XCI library easily

## Features
* [All XCI Explorer features](https://github.com/StudentBlake/XCI-Explorer/blob/master/README.md)
* Displays your whole library
* Makes managing easy

## Requirements
* Visual Studio 2017
* [Hactool](https://github.com/SciresM/hactool/releases)
* [Dumped keys](https://gbatemp.net/threads/how-to-get-switch-keys-for-hactool-xci-decrypting.506978/) ([optional](https://github.com/StudentBlake/XCI-Explorer/releases/download/v1.0.0.0/Get-keys.txt.bat))

![main](https://imgur.com/RV2EGB6.jpg)

## Latest Changelog
v0.0.8rev003 [PRE-RELEASE]
* [FEATURE] Added cache to reduce load times
* Program won't freeze while loading files! (needs testing with large library)
* Added status indicator in the Tools tab
* Properly sort list by game name
* Other fixes and changes I already forgot
* [NOTE] A lot has changed under the hood, there may be bugs and so this is a PRE-RELEASE
* [DEV-NOTE] The code is getting messy fast. We need to take some time and organize things

## Build Instructions
* Open `XCI Organizer.sln`
* Build -> Build Solution
* Add hactool.exe + dependencies + keys.txt to `XCI-Organizer/bin/Debug/` folder
* Run `XCI-Organizer.exe`

## Special Thanks
* klks - CARD2, Hash Validation, bug fixes
* gibaBR - XCI Organizer creator, features, refactoring code, bug fixes
* StudentBlake - Special contributor to the project

## Disclaimer
This software is unfinished and unoptimized! Please use with caution.
