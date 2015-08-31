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

IF "%SKIP_KRE_INSTALL%"=="1" goto run
REM CALL packages\KoreBuild\build\dnvm upgrade -runtime CLR -x86 || set errorlevel=1
REM CALL packages\KoreBuild\build\dnvm install 1.0.0-beta2 -runtime CoreCLR -x86 || set errorlevel=1

@powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"
CALL %USERPROFILE%\.dnx\bin\dnvm install latest -runtime CLR -x86 -alias default || set errorlevel=1
CALL %USERPROFILE%\.dnx\bin\dnvm install latest -runtime CoreCLR -x86 || set errorlevel=1

:run
REM CALL packages\KoreBuild\build\dnvm use default -runtime CLR -x86 || set errorlevel=1
REM packages\Sake\tools\Sake.exe -I packages\KoreBuild\build -f makefile.shade %*

CALL %USERPROFILE%\.dnx\bin\dnvm use default -runtime CLR -x86 || set errorlevel=1

CALL dnu restore || set errorlevel=1
CALL dnu build AspNet5 || set errorlevel=1
CALL dnu pack AspNet5 || set errorlevel=1

