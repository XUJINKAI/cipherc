rmdir /S /Q publish-linux
rmdir /S /Q publish-win
dotnet publish -c Release -r linux-x64 -o publish-linux /p:PublishSingleFile=true --no-self-contained cipherc/cipherc.csproj
dotnet publish -c Release -r win-x64 -o publish-win /p:PublishSingleFile=true --no-self-contained cipherc/cipherc.csproj
