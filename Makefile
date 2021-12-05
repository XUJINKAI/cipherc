PUB_WIN_DIR := publish-win-x64
PUB_LINUX_DIR := publish-linux-x64

all: linux win

publish: linux win
	tar -zcvf cipherc-linux-x64.tar.gz $(PUB_LINUX_DIR)
	tar -zcvf cipherc-win-x64.tar.gz $(PUB_WIN_DIR)

version:
	-rm ./cipherc/ENV.gen.cs
	./cipherc/ENV.gen.sh

win: version
	dotnet publish -c Release -r win-x64 -o $(PUB_WIN_DIR) /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:PublishTrimmed=false /p:Product=cipherc-%GITVERSION% --self-contained cipherc/cipherc.csproj

linux: version
	dotnet publish -c Release -r linux-x64 -o $(PUB_LINUX_DIR) /p:PublishSingleFile=true /p:PublishTrimmed=false --self-contained cipherc/cipherc.csproj

run: version
	dotnet run --project cipherc/cipherc.csproj
