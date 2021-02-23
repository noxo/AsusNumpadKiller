# AsusNumpadKiller

Got shiny new Asus Gaming RTX 30 gaming laptop. But it has one annoying feature; NumPad which is integrated
into touchpad. https://www.asus.com/support/FAQ/1038283/

That's right, clicking on top-right corner of touchpad turns touchpad into numeric keyboard and stops controlling the pointer. 
It's super annoying when you turn it on accidentally, and of course there is no way to turn this feature of permanenly.

## How does it work
It's a simple 7kb executable which hooks into Windows keyboard system, and consumes numlock events.

Included is also subproject which builds .MSI installer that will:
- copy the executable in Program files & startup folder
- start the executable once installer is complete

## Building requirements 
- Visual Studio 2019 Community
- Visual Studio Installer Project Extension https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2017InstallerProjects

## Building instructions
- Open project in Visual Studio
- Right-click install project -> Install
- Alternatively you can just build AsusNumpadKiller project and run the executable from Debug/Release folder

## Alternatives
- https://www.carehart.org/blog/client/index.cfm/2020/12/20/how_to_disable_asus_numberpad
- uninstall Asus Numpber Pad device driver in device manager (will be back after restart)
- Microsoft PowerToys, remap Numlock -> Shift

## Troubleshooting
Executable can be manually killed from Task Manager -> Details -> Locate "AsusNumpadKiller.exe" -> Right click -> End task
