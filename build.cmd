@echo off
cd %~dp0

SETLOCAL ENABLEEXTENSIONS
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
IF EXIST packages\KoreBuild goto run
.nuget\NuGet.exe install KoreBuild -ExcludeVersion -o packages -nocache -pre -Source https://www.nuget.org/api/v2/;https://www.myget.org/F/aspnetvnext/
.nuget\NuGet.exe install Sake -version 0.2 -o packages -ExcludeVersion -Source https://www.nuget.org/api/v2/;https://www.myget.org/F/aspnetvnext/

@powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"
CALL %USERPROFILE%\.dnx\bin\dnvm install latest -runtime CLR -arch x86 -alias default || set errorlevel=1
CALL %USERPROFILE%\.dnx\bin\dnvm install latest -runtime CoreCLR -arch x86 || set errorlevel=1

CALL %USERPROFILE%\.dnx\bin\dnvm use default -runtime CLR -x86 || set errorlevel=1

CALL dnu restore -s https://www.myget.org/F/aspnetvnext/ -f https://www.nuget.org/api/v2/ || set errorlevel=1

CALL dnu pack SimpleConfigService.AspNet5 --configuration Release || set errorlevel=1

