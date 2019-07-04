dotnet publish ..\src\Ordering.App -c Release
MSBuild.exe ..\src\Ordering.DB\ /t:Build /p:Configuration=Release