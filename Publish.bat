dotnet publish -c Release -r win-x64 -f netcoreapp3.0 /p:PublishSingleFile=true /p:DebugType=None /p:PublishTrimmed=true -o pub RTScript
pause