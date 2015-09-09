curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
dnvm install latest -runtime CoreCLR -arch x86 
dnvm use default -runtime CoreCLR -arch x86

dnu restore -s https://www.myget.org/F/aspnetvnext/ -f https://www.nuget.org/api/v2/ || set errorlevel=1
dnu pack SimpleConfigService.AspNet5 --configuration Release || set errorlevel=1
