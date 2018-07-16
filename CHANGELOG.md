# Changelog
**v0.1.2 EXPERIMENTAL**
* Support for NSWDB revisions
* Cache is now universal and can be shared between users
* Trimming no longer deletes cache
* Reworked some code
* A lot of under the hood changes, something may be broken
* This release may be slightly slower because file sizes and other info is retrieved every refresh (needs testing with large library)

v0.1r1 FINAL
* Game icon properly sized
* Batch rename now takes less than 5 seconds
* Cache changes (will have to be rewritten)
* This is probably the last release for this project (a better version coming soon)

v0.0.9rev1 [PRE-RELEASE]
* [FEATURE] Custom batch file renaming schemes thanks to RedSparr0w
* [FEATURE] Basic ascending sorting by ID or Game Name
* Added more information to list

v0.0.8rev004 [PRE-RELEASE]
* [FEATURE] Added cache to reduce load times
* Program won't freeze while loading files! (needs testing with large library)
* Added status indicator in the Tools tab
* Properly sort list by game name
* Reorder columns (won't save)
* Other fixes and changes I already forgot
* [NOTE] A lot has changed under the hood, there may be bugs and so this is a PRE-RELEASE
* [DEV-NOTE] The code is getting messy fast. We need to take some time and organize things

v0.0.7 [PRE-RELEASE]
* New user interface for listing games!
* Highlights when files are not trimmed
* Context menu available in the games list (Right click on it!)
* Some code refactoring
* Some under the hood improvements
* [KNOWN ISSUE] Struggles loading with larger game libraries

v0.0.6
* You can now choose between Simple, Detailed, and Scene naming schemes for batch renaming
* Note: If you are getting errors related to XML, try using the "Update NSWDB" button in the Tool menu.

v0.0.5
* [FEATURE] By default, uses scene release name from NSWDB for batch rename
* Faster solution for downloading missing files

v0.0.4
* [FEATURE] Click on picture to save it to current directory
* Fix batch renaming files incorrectly (temporary renaming solution)
* Fix batch trimming destroying files
* Minor bug fixes
* NSWDB renaming support coming soon(tm)

v0.0.3
* Batch file renaming/trimming
* Fixed potential crash

v0.0.2
* Game names are displayed instead of path names
* Fixed bug where base directory with folders would only display folder contents
* Disabled cert import/clear buttons while extracting NCA (latest XCI Explorer)

v0.0.1
* Initial release