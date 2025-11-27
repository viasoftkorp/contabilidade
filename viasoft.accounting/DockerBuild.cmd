git pull --prune
dotnet build -c Release .\Viasoft.Accounting.sln
dotnet publish -c Release
cd .\Viasoft.Accounting.Host\
docker build --no-cache -t korpcicd/viasoft.accounting:latest .
docker push korpcicd/viasoft.accounting:latest
pause