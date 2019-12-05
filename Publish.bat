dotnet publish -c Release -r win-x64 -f netcoreapp3.0 /p:DebugType=None -o publish rtscript --self-contained false
pause