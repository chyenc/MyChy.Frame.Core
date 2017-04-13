del nupkgs\*.nupkg
dotnet pack MyChy.Frame.Core.Common\MyChy.Frame.Core.Common.csproj -c Release -o ..\nupkgs\
dotnet pack MyChy.Frame.Core.Redis\MyChy.Frame.Core.Redis.csproj  -c Release -o ..\nupkgs\
dotnet pack MyChy.Frame.Core.EFCore\MyChy.Frame.Core.EFCore.csproj  -c Release -o ..\nupkgs\

nuget push nupkgs\*.nupkg -s http://nuget.chyenc.com 8bfc20c1-83a6-4d0f-a48e-209a8dda7ad2

